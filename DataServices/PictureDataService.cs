using Data;
using Data.Model;
using System;
using System.IO;
using System.Web;

namespace Services
{
    public class PictureDataService : IPictureService
    {
        const string imageDirName = "images";
        string imageDirectoryPath => Path.Combine(Directory.GetCurrentDirectory(), imageDirName);

        private PicsurferContext _context;
        public PictureDataService(PicsurferContext context)
        {
            _context = context;
        }

        public void SetUniquePath(Picture picture)
        {
            picture.Path = Path.Combine(imageDirectoryPath, Guid.NewGuid() + picture.Name);
        }

        public void SavePicture(Picture picture, HttpPostedFileBase upload)
        {
            SetUniquePath(picture);
            upload.SaveAs(picture.Path);
        }

        public void Create(Picture picture, HttpPostedFileBase uploadFile)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    SavePicture(picture, uploadFile);
                    _context.Pictures.Add(picture);
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
    }
}
