using Data;

namespace Services
{
    public class PictureDataService : IPictureService
    {
        private PicsurferContext _context;
        public PictureDataService(PicsurferContext context)
        {
            _context = context;
        }
    }
}
