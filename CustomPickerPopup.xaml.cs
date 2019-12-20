using Plugin.Shared.Models;
using Quorum.FieldVisor.Mobile.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quorum.FieldVisor.Mobile.Pages.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomPickerPopup : PopupPage
    {
        private TaskCompletionSource<string> taskCompletion;
        public List<PickerItem> Items;
        public CustomPickerPopup(List<string> items)
        {
            Items = new List<PickerItem>();

            foreach (var item in items)
            {
                if (item != null)
                {
                    Items.Add(new PickerItem()
                    {
                        Value = item
                    });
                }

            }
            InitializeComponent();
            mainList.ItemsSource = Items;
            
            int wrappedItems = 0;

            foreach (var item in Items)
            {
                if ((item.Value.Length > 30))
                    wrappedItems++;
            }
            mainList.HeightRequest = (Items.Count() + wrappedItems * 0.5) > 9 ? 463.5 : 51.5 * (Items.Count() + wrappedItems * 0.5);
        }

        public static async Task<string> GetSelectedPickerItem(object obj = null)
        {
            List<string> pickerItems = new List<string>();
         
            if (obj is IEnumerable<string>)
            {
                pickerItems = ((IEnumerable<string>)obj).ToList();
            }
            else if (obj is IEnumerable<SelectListItem>)
            {
                foreach (var item in (IEnumerable<SelectListItem>)obj)
                {
                    pickerItems.Add(item.Name);
                }
            }
            

            CustomPickerPopup pop = new CustomPickerPopup(pickerItems);

            var result = await pop.GetSelectedPickerItemInternal(pickerItems);

            return result;
        }

        private async Task<string> GetSelectedPickerItemInternal(List<string> pickerItems)
        {
            taskCompletion = new TaskCompletionSource<string>();

            await PopupNavigation.Instance.PushAsync(this);

            return await taskCompletion.Task;
        }

        private async void MainList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PickerItem item = e.SelectedItem as PickerItem;

            if (taskCompletion != null)
            {
                taskCompletion.TrySetResult(item.Value.ToString());

                taskCompletion = null;
            }
            await PopupNavigation.Instance.PopAsync();
        }

        private async void BackgroundTapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }

    public class PickerItem
    {
        private string value;

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

    }
}
