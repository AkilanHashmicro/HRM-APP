using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static HRMAPP.Model.HRMModel;

namespace HRMAPP.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FileAttachmentPage : PopupPage
	{
        List<AttachmentFile> attach = new List<AttachmentFile>();
		public FileAttachmentPage ()
		{
			InitializeComponent ();

            var camera_recognizer = new TapGestureRecognizer();
            camera_recognizer.Tapped += (s, e) =>
            {
                MessagingCenter.Send<string, bool>("MyApp", "cameraMsg", true);
                PopupNavigation.Instance.PopAsync();

            };
            camera_click.GestureRecognizers.Add(camera_recognizer);

            var gallery_recognizer = new TapGestureRecognizer();
            gallery_recognizer.Tapped += (s, e) =>
            {
                MessagingCenter.Send<string, bool>("MyApp", "galleryMsg", true);
                PopupNavigation.Instance.PopAsync();
            };
            gallery_click.GestureRecognizers.Add(gallery_recognizer);


        }

        public FileAttachmentPage(string page, List<AttachmentFile> attachnew)
        {
            InitializeComponent();

            attach = attachnew;

            var camera_recognizer = new TapGestureRecognizer();
            camera_recognizer.Tapped += async (s, e) =>
            {

                AttachmentFile data;

                try
                {
                    data = new AttachmentFile();

                    var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

                    if (photo == null)
                    {

                    }

                    if (photo != null)
                    {
                        byte[] bydata;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            photo.GetStream().CopyTo(ms);
                            bydata = ms.ToArray();
                        }
                        string b64string = Convert.ToBase64String(bydata);
                        var path = photo.Path;
                        var filepath = photo.AlbumPath;
                        string filename = path.Substring(path.LastIndexOf("/") + 1);

                        data.file_name = filename;
                        data.filepath = filepath;
                        data.filebase64 = b64string;

                        attach.Add(data);
                    }

                }
                catch (Exception ex)
                {
                  //  filedata = null;
                    System.Diagnostics.Debug.WriteLine("Warning Exception :  " + ex.Message);
                }

                //   MessagingCenter.Send<string, bool>("MyApp", "updatecameraMsg", true);
                MessagingCenter.Send<string, List<AttachmentFile>>("MyApp", "updatecameraMsg", attach);

                await  PopupNavigation.Instance.PopAsync();

            };
            camera_click.GestureRecognizers.Add(camera_recognizer);

            var gallery_recognizer = new TapGestureRecognizer();
            gallery_recognizer.Tapped += async (s, e) =>
            {

                AttachmentFile data;

                FileData fileData = new FileData();
                FileData filedata = null;
                try
                {
                    data = new AttachmentFile();

                    string fileName = "";

                    filedata = await CrossFilePicker.Current.PickFile();
                    if (!string.IsNullOrEmpty(filedata.FileName))   //Just the file name, it doesn't has the path
                    {
                        byte[] bydata = filedata.DataArray;
                        fileName = filedata.FileName;
                        string path = filedata.FilePath;
                        string contents = System.Text.Encoding.UTF8.GetString(filedata.DataArray);
                        string base64 = Convert.ToBase64String(bydata);

                        var dat = filedata.GetStream();

                        if (fileName.Contains(".doc"))
                        {
                            data.file_name = fileName;
                            // data.filepath = path;
                            data.file_data = base64;
                            // data.filebase64 = base64;
                            System.Diagnostics.Debug.WriteLine("File Name" + fileName);
                            System.Diagnostics.Debug.WriteLine("File Date" + contents);
                        }
                        else if (fileName.Contains(".png") || fileName.Contains(".jpg"))
                        {
                            data.file_name = fileName;
                            //  data.filepath = path;
                            data.file_data = base64;
                            // data.filebase64 = base64;
                            System.Diagnostics.Debug.WriteLine("File Name" + fileName);
                            System.Diagnostics.Debug.WriteLine("File Date" + contents);
                        }
                        else if (fileName.Contains(".pdf"))
                        {
                            data.file_name = fileName;
                            //  data.filepath = path;
                            data.file_data = base64;
                            //  data.filebase64 = base64;
                            System.Diagnostics.Debug.WriteLine("File Name" + fileName);
                            System.Diagnostics.Debug.WriteLine("File Date" + contents);
                        }
                        else if (fileName.Contains(".txt"))
                        {
                            data.file_name = fileName;
                            //  data.filepath = path;
                            data.file_data = base64;
                            //  data.filebase64 = base64;
                            System.Diagnostics.Debug.WriteLine("File Name" + fileName);
                            System.Diagnostics.Debug.WriteLine("File Date" + contents);
                        }
                        attach.Add(data);
                    }


                    attach = attach.GroupBy(i => i.file_name).Select(g => g.First()).ToList();

                }
                catch (Exception ex)
                {
                    filedata = null;
                    System.Diagnostics.Debug.WriteLine("Warning Exception :  " + ex.Message);
                }

                MessagingCenter.Send<string, List<AttachmentFile>>("MyApp", "updategalleryMsg", attach);
                await PopupNavigation.Instance.PopAsync();
            };
            gallery_click.GestureRecognizers.Add(gallery_recognizer);



        }

    }
}