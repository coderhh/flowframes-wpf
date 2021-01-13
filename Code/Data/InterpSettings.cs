﻿using Flowframes.AudioVideo;
using Flowframes.Data;
using Flowframes.IO;
using Flowframes.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Flowframes
{
    public struct InterpSettings
    {
        public string inPath;
        public string outPath;
        public AI ai;
        public float inFps;
        public float outFps;
        public int interpFactor;
        public Interpolate.OutMode outMode;
        public string model;

        public string tempFolder;
        public string framesFolder;
        public string interpFolder;
        public bool inputIsFrames;
        public string outFilename;
        public Size inputResolution;
        public Size scaledResolution;

        public InterpSettings(string inPathArg, string outPathArg, AI aiArg, float inFpsArg, int interpFactorArg, Interpolate.OutMode outModeArg, string modelArg)
        {
            inPath = inPathArg;
            outPath = outPathArg;
            ai = aiArg;
            inFps = inFpsArg;
            interpFactor = interpFactorArg;
            outFps = inFpsArg * interpFactorArg;
            outMode = outModeArg;
            model = modelArg;

            try
            {
                tempFolder = InterpolateUtils.GetTempFolderLoc(inPath, outPath);
                framesFolder = Path.Combine(tempFolder, Paths.framesDir);
                interpFolder = Path.Combine(tempFolder, Paths.interpDir);
                inputIsFrames = IOUtils.IsPathDirectory(inPath);
                outFilename = Path.Combine(outPath, Path.GetFileNameWithoutExtension(inPath) + IOUtils.GetExportSuffix(interpFactor, ai, model) + FFmpegUtils.GetExt(outMode));
            }
            catch
            {
                Logger.Log("Tried to create InterpSettings struct without an inpath. Can't set tempFolder, framesFolder and interpFolder.", true);
                tempFolder = "";
                framesFolder = "";
                interpFolder = "";
                inputIsFrames = false;
                outFilename = "";
            }

            inputResolution = new Size(0, 0);
            scaledResolution = new Size(0, 0);
        }

        public void UpdatePaths (string inPathArg, string outPathArg)
        {
            inPath = inPathArg;
            outPath = outPathArg;
            tempFolder = InterpolateUtils.GetTempFolderLoc(inPath, outPath);
            framesFolder = Path.Combine(tempFolder, Paths.framesDir);
            interpFolder = Path.Combine(tempFolder, Paths.interpDir);
            inputIsFrames = IOUtils.IsPathDirectory(inPath);
            outFilename = Path.Combine(outPath, Path.GetFileNameWithoutExtension(inPath) + IOUtils.GetExportSuffix(interpFactor, ai, model) + FFmpegUtils.GetExt(outMode));
        }

        public Size GetInputRes ()
        {
            RefreshResolutions();
            return inputResolution;
        }

        public Size GetScaledRes()
        {
            RefreshResolutions();
            return scaledResolution;
        }

        void RefreshResolutions ()
        {
            if (inputResolution.IsEmpty || scaledResolution.IsEmpty)
            {
                inputResolution = IOUtils.GetVideoRes(inPath);
                scaledResolution = InterpolateUtils.GetOutputResolution(inputResolution, false);
            }
        }

        public int GetTargetFrameCount(string overrideInputDir = "", int overrideFactor = -1)
        {
            if (framesFolder == null || !Directory.Exists(framesFolder))
                return 0;

            string framesDir = (!string.IsNullOrWhiteSpace(overrideInputDir)) ? overrideInputDir : framesFolder;
            int frames = IOUtils.GetAmountOfFiles(framesDir, false, "*.png");
            int factor = (overrideFactor > 0) ? overrideFactor : interpFactor;
            int targetFrameCount = (frames * factor) - (interpFactor - 1);
            return targetFrameCount;
        }
    }
}
