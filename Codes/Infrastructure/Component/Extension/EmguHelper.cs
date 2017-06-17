using System.Drawing;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Component.Extension
{
    public static class EmguHelper
    {
        /// <summary>
        /// 比较图像
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static double CompareImage(string path1, string path2)
        {
            using (Image<Bgr, byte> a = new Image<Bgr, byte>(path1))
            {
                using (Image<Bgr, byte> b = new Image<Bgr, byte>(path2))
                {
                    Image<Gray, float> c = new Image<Gray, float>(a.Width, a.Height);
                    c = a.MatchTemplate(b, TemplateMatchingType.CcorrNormed);
                    double min = 0, max = 0;
                    Point maxp = new Point(0, 0);
                    Point minp = new Point(0, 0);
                    CvInvoke.MinMaxLoc(c, ref min, ref max, ref minp, ref maxp);
                    c.Dispose();
                    return max;
                }
            }
        }

        /// <summary>
        /// 比较图像
        /// </summary>
        /// <param name="bytelist1"></param>
        /// <param name="bytelist2"></param>
        /// <returns></returns>
        public static double CompareImage(byte[] bytelist1, byte[] bytelist2)
        {
            MemoryStream ms1 = new MemoryStream(bytelist1);
            Bitmap bm1 = (Bitmap)Image.FromStream(ms1);
            MemoryStream ms2 = new MemoryStream(bytelist2);
            Bitmap bm2 = (Bitmap)Image.FromStream(ms2);
            using (Image<Bgr, byte> a = new Image<Bgr, byte>(bm1))
            {
                using (Image<Bgr, byte> b = new Image<Bgr, byte>(bm2))
                {
                    Image<Gray, float> c = new Image<Gray, float>(a.Width, a.Height);
                    c = a.MatchTemplate(b, TemplateMatchingType.CcorrNormed);
                    double min = 0, max = 0;
                    Point maxp = new Point(0, 0);
                    Point minp = new Point(0, 0);
                    CvInvoke.MinMaxLoc(c, ref min, ref max, ref minp, ref maxp);
                    c.Dispose();
                    return max;
                }
            }
        }
    }
}
