using Data;
using Data.Model;
using DataService.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Services
{
    public class PictureDataService : IPictureService
    {
        const string ImageDirName = "images";
        string ImageDirectoryPath => Path.Combine(Directory.GetCurrentDirectory(), ImageDirName);

        private PicsurferContext _context;
        public PictureDataService(PicsurferContext context)
        {
            _context = context;
        }

        public PictureDataService(string connectionStr)
        {
            _context = new PicsurferContext(connectionStr);
        }

        public string SetUniquePath(string fileName)
        {
            return Path.Combine(ImageDirectoryPath, Guid.NewGuid() + fileName);
        }

        public Picture PictureEntityFromFile(HttpPostedFileBase upload)
        {
            return new Picture() {
                Extension = Path.GetExtension(upload.FileName),
                Path = SetUniquePath(upload.FileName),
                Name = upload.FileName,
                Rates = null
            };
        }

        public void SavePicturesFromFiles(HttpPostedFileBase[] filesToUpload)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var uploadedFile in filesToUpload)
                    {
                        var picture = PictureEntityFromFile(uploadedFile);
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
