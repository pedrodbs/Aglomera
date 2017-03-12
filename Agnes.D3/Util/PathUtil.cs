// ------------------------------------------
// <copyright file="PathUtil.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes.D3
//    Last updated: 2017/03/10
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Agnes.D3.Util
{
    public static class PathUtil
    {
        #region Public Methods

        public static string AddFilePrefix(string fileFullPath, string prefix)
        {
            return
                $"{Path.GetDirectoryName(fileFullPath)}{Path.DirectorySeparatorChar}" +
                $"{prefix}{Path.GetFileName(fileFullPath)}";
        }

        public static string AddFileSuffix(string fileFullPath, string suffix)
        {
            return
                $"{Path.GetDirectoryName(fileFullPath)}{Path.DirectorySeparatorChar}" +
                $"{Path.GetFileNameWithoutExtension(fileFullPath)}{suffix}{Path.GetExtension(fileFullPath)}";
        }

        public static void CheckDeleteFile(string filePath, bool createDirectory = false)
        {
            if (!File.Exists(filePath) && !createDirectory) return;

            try
            {
                if (File.Exists(filePath)) File.Delete(filePath);
                if (createDirectory) CreateDirectory(Path.GetDirectoryName(filePath));
            }
            catch (IOException)
            {
            }
        }

        public static void ClearDirectory(string path)
        {
            if (!Directory.Exists(path)) return;

            //recursive clean in sub-directories
            foreach (var directory in Directory.GetDirectories(path))
                ClearDirectory(directory, true);

            //sends file to recycle bin
            foreach (var file in Directory.GetFiles(path))
                File.Delete(file);
        }

        public static void ClearDirectory(string path, bool deleteDirs)
        {
            if (!Directory.Exists(path)) return;

            //recursive clean in sub-directories
            foreach (var directory in Directory.GetDirectories(path))
                ClearDirectory(directory, deleteDirs);

            //sends file to recycle bin
            foreach (var file in Directory.GetFiles(path))
                File.Delete(file);

            //RecybleBinUtil.SendSilent(file);

            if (!deleteDirs) return;

            //also deletes the directory itself
            while (Directory.GetFiles(path).Length != 0)
                Thread.Sleep(10);

            if (!Directory.Exists(path)) return;

            try
            {
                Directory.Delete(path);
            }
            catch (IOException)
            {
            }
        }

        public static void ClearEmptyDirectories(string path)
        {
            if (path == null || !Directory.Exists(path)) return;
            if (IsDirectoryEmpty(path)) Directory.Delete(path);
            else ClearEmptyDirectories(Path.GetDirectoryName(path));
        }

        public static void CreateDirectory(string path)
        {
            //checks directory for creation
            if (path != null && !Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void CreateOrClearDirectory(string path)
        {
            //creates and clears directory
            CreateDirectory(path);
            ClearDirectory(path);
        }

        public static void DeleteAll(List<string> filesToDelete)
        {
            //verifies list
            if (filesToDelete == null || filesToDelete.Count == 0) return;

            //deletes all files on list and tries to delete their directories if empty
            foreach (var file in filesToDelete)
            {
                var dir = Path.GetDirectoryName(file);
                CheckDeleteFile(file);
                if (dir != null && IsDirectoryEmpty(dir))
                    ClearEmptyDirectories(dir);
            }
        }

        public static string GetRelativePath(string fullPath)
        {
            return GetRelativePath(fullPath, Path.GetFullPath("."));
        }

        public static string GetRelativePath(string fullPath, string basePath)
        {
            if (string.IsNullOrEmpty(fullPath) || string.IsNullOrEmpty(basePath)) return null;
            if (fullPath.Equals(basePath)) return string.Empty;

            var pathUri = new Uri(fullPath);

            // Folders must end in a slash
            if (!basePath.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture),
                StringComparison.Ordinal))
                basePath += Path.DirectorySeparatorChar;

            var basePathUri = new Uri(basePath);
            return Uri.UnescapeDataString(
                basePathUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public static string GetValidFileName(string fileName, char replacement = '_')
        {
            if (string.IsNullOrWhiteSpace(fileName)) return fileName;
            var invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            var invalidReStr = $"[{invalidChars}]+";
            return Regex.Replace(fileName, invalidReStr, replacement.ToString());
        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.GetFileSystemEntries(path).Any();
        }

        public static string ReplaceExtension(string fileName, string newExtension)
        {
            return
                $"{Path.GetDirectoryName(fileName)}{Path.DirectorySeparatorChar}{Path.GetFileNameWithoutExtension(fileName)}.{newExtension}";
        }

        public static string ReplaceInvalidChars(string fileName, char replacer)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return invalidChars.Aggregate(fileName, (current, invalidChar) => current.Replace(invalidChar, replacer));
        }

        public static bool VerifyFilePathCreation(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return false;
            var directoryName = Path.GetDirectoryName(filePath);
            if (directoryName.Equals(string.Empty)) return true;
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            if (File.Exists(filePath))
                File.Delete(filePath);
            return true;
        }

        #endregion
    }
}