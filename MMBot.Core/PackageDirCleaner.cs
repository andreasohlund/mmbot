using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MMBot
{
    public static class PackageDirCleaner
    {
        const string CleanUpFilePath = @".\packages-to-cleanup.txt";
        public static void CleanUpPackages()
        {
            if (!File.Exists(CleanUpFilePath))
            {
                return;
            }

            var dirsToDelete = File.ReadAllLines(CleanUpFilePath);

            foreach (var dir in dirsToDelete.Where(Directory.Exists))
            {
                Directory.Delete(dir, true);
            }

            File.Delete(CleanUpFilePath);
        }

        public static void RegisterDirectoriesToDelete(IEnumerable<string> packageFoldersToDelete)
        {
            File.WriteAllLines(CleanUpFilePath, packageFoldersToDelete);
        }
    }
}