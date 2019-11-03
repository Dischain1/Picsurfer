using Data;
using Data.Model;
using DataService.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Web;

namespace Services
{
    public class PictureDataService : IPictureService
    {
        const string ImageDirName = "images";

        private PicsurferContext _context;
        public PictureDataService(PicsurferContext context)
        {
            _context = context;
        }

        public PictureDataService(string connectionStr)
        {
            _context = new PicsurferContext(connectionStr);
        }

        public Picture PictureEntityFromFile(HttpPostedFileBase upload, string baseDir)
        {
            var uniqueName = $"{Guid.NewGuid()}{upload.FileName}";
            var directoryPath = Path.Combine(baseDir, ImageDirName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return new Picture() {
                Extension = Path.GetExtension(upload.FileName),
                Path = Path.Combine(baseDir, ImageDirName, uniqueName),
                Name = uniqueName,
                Rates = null
            };
        }

        public void SavePicturesFromFiles(HttpPostedFileBase[] filesToUpload, string baseDir)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var uploadedFile in filesToUpload)
                    {
                        var picture = PictureEntityFromFile(uploadedFile, baseDir);
                        uploadedFile.SaveAs(picture.Path);

                        _context.Pictures.Add(picture);
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

        public bool AreImages(HttpPostedFileBase[] filesToUpload)
        {
            return filesToUpload.ToList().All(file => file.IsImage());
        }
    }
}
