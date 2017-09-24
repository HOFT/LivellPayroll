using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.QrCode;

namespace LivellPayRoll.App_Helpers
{
    public class ZXingHelpers
    {
        /// <summary>
        /// 生成二维码,保存成图片
        /// </summary>
        public static void Generate1(string content, string dirPath)
        {
            //string QRCodePath = ConfigurationManager.AppSettings["QRCodePath"];
            //dirpath = dirpath + QRCodePath;
            //string dirpath = "C:\\QRCode\\";
            //string dirpath = Request.ApplicationPath "C:\\Hoya Project\\ASP.NetLivellPayRoll\\webapp\\Content\\img\\QRCode\\";
            //if (!Directory.Exists(dirpath))
            //    Directory.CreateDirectory(dirpath);
            //string filename = dirPath + fileName + ".png";

            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            QrCodeEncodingOptions options = new QrCodeEncodingOptions();
            options.DisableECI = true;
            //设置内容编码
            options.CharacterSet = "UTF-8";
            //设置二维码的宽度和高度
            options.Width = 500;
            options.Height = 500;
            //设置二维码的边距,单位不是固定像素
            options.Margin = 1;
            writer.Options = options;

            Bitmap map = writer.Write(content);

            map.Save(dirPath, ImageFormat.Png);
            map.Dispose();
        }
        /// <summary>
        /// 读取二维码
        /// 读取失败，返回空字符串
        /// </summary>
        /// <param name="filename">指定二维码图片位置</param>
        public static string Read1(string filename)
        {
            BarcodeReader reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            Bitmap map = new Bitmap(filename);
            Result result = reader.Decode(map);
            return result == null ? "" : result.Text;
        }
        public static void FileDelte(string filename) {
            if (File.Exists(filename))
            {
                //参数1：要删除的文件路径
                File.Delete(filename);
            }
        }
    }
}