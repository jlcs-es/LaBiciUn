using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace LaBiciUn
{
    class CustomNotifications
    {

        public async static void displayInfoDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                PrimaryButtonText = "Ok"
            };

            ContentDialogResult result = await dialog.ShowAsync();
        }
    }
}
