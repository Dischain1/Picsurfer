using Data;
using System;
using System.IO;
using System.Web;

namespace Services
{
    public class FileDataService
    {
        const string FilesDirName = "files";

        private FileHubContext _context;
        public FileDataService(FileHubContext context)
        {
            _context = context;
        }

        public FileDataService(string connectionStr)
        {
            _context = new FileHubContext(connectionStr);
        }

        public Data.Model.File ConstructFileDbEntity(HttpPostedFileBase upload, string baseDir)
        {
            var uniqueName = $"{Guid.NewGuid()}{upload.FileName}";
            var directoryPath = Path.Combine(baseDir, FilesDirName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return new Data.Model.File() {
                Extension = Path.GetExtension(upload.FileName),
                Path = Path.Combine(baseDir, FilesDirName, uniqueName),
                Name = upload.FileName,
            };
        }

        public void SaveFiles(HttpPostedFileBase[] filesToUpload, string baseDir)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var uploadedFile in filesToUpload)
                    {
                        var file = ConstructFileDbEntity(uploadedFile, baseDir);
                        uploadedFile.SaveAs(file.Path);

                        _context.Files.Add(file);
                    }
                    
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

                transaction.Commit();
            }
        }

        public void Delete(int fileId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Data.Model.File file = _context.Files.Find(fileId);
                    _context.Files.Remove(file);
                    _context.SaveChanges();

                    System.IO.File.Delete(file.Path);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

        }
    }
}
