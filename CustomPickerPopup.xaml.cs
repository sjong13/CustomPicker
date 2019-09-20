using Quorum.FieldVisor.Mobile.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
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
        public FloatLabeledEntry FLE;
        public List<PickerItem> Items;
        public CustomPickerPopup(List<string> items, FloatLabeledEntry fle = null)
        {
            FLE = fle;
            if (FLE == null)
            {
                FLE = new FloatLabeledEntry();
            }
            FLE.UnFocusEntry();
            Items = new List<PickerItem>();

            foreach (var item in items)
            {
                Items.Add(new PickerItem()
                {
                    Value = item
                });
            }
            InitializeComponent();
            mainList.ItemsSource = Items;
            int wrappedItems = 0;

            foreach (var item in Items)
            {
                if ((item.Value.Length > 27))
                    wrappedItems++;
            }
            mainList.HeightRequest = (Items.Count() + wrappedItems * 0.5) > 9 ? 400 : 44 * (Items.Count() + wrappedItems * 0.5);
        }

        private async void MainList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PickerItem item = e.SelectedItem as PickerItem;
            FLE.Text = item.Value.ToString();
            FLE.SelectedIndex = Items.IndexOf(Items.Where(x=>x.Value == item.Value.ToString()).FirstOrDefault());
            FLE.UnFocusEntry();
            await PopupNavigation.Instance.PopAsync();
        }

        private void PopupPage_Disappearing(object sender, EventArgs e)
        {
            FLE.UnFocusEntry();
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