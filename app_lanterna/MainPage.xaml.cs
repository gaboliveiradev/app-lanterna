using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Plugin.Battery;
using Xamarin.Essentials;
using System.Runtime.ConstrainedExecution;

namespace app_lanterna
{
    public partial class MainPage : ContentPage
    {
        bool ligada = false;
        public MainPage()
        {
            InitializeComponent();

            bnt_on_off.Source = ImageSource.FromResource("app_lanterna.imagens.switch-off.png");
            toChargeInfoBattery();
        }

        private async void toChargeInfoBattery()
        {
            try
            {
                if(CrossBattery.IsSupported)
                {
                    CrossBattery.Current.BatteryChanged -= chargeStatusBattery;
                    CrossBattery.Current.BatteryChanged += chargeStatusBattery;
                } else
                {
                    lbl_bateria_aviso.Text = "As Informações sobre a bateria não estão disponíveis";
                }
            } catch (Exception err)
            {
                await DisplayAlert("Ocorreu um erro: \n", err.Message, "OK");
            }
        }

        private async void chargeStatusBattery(object sender, Plugin.Battery.Abstractions.BatteryChangedEventArgs e)
        {
            try
            {
                lbl_porcentagem.Text = e.RemainingChargePercent.ToString() + "%";

                if(e.IsLow)
                {
                    lbl_bateria_aviso.Text = "Atenção! A Bateria está Fraca!";
                } else
                {
                    lbl_bateria_aviso.Text = "";
                }

                switch(e.Status)
                {
                    case Plugin.Battery.Abstractions.BatteryStatus.Charging:
                        lbl_status.Text = "Carregando...";
                        break;
                    case Plugin.Battery.Abstractions.BatteryStatus.Discharging:
                        lbl_status.Text = "Desconectado";
                        break;
                    case Plugin.Battery.Abstractions.BatteryStatus.Full:
                        lbl_status.Text = "Carregada";
                        break;
                    case Plugin.Battery.Abstractions.BatteryStatus.NotCharging:
                        lbl_status.Text = "Sem Carregar";
                        break;
                    case Plugin.Battery.Abstractions.BatteryStatus.Unknown:
                        lbl_status.Text = "Desconhecido";
                        break;
                }

                switch(e.PowerSource)
                {
                    case Plugin.Battery.Abstractions.PowerSource.Ac:
                        lbl_fonte.Text = "Carregador";
                        break;
                    case Plugin.Battery.Abstractions.PowerSource.Battery:
                        lbl_fonte.Text = "Bateria";
                        break;
                    case Plugin.Battery.Abstractions.PowerSource.Usb:
                        lbl_fonte.Text = "USB";
                        break;
                    case Plugin.Battery.Abstractions.PowerSource.Wireless:
                        lbl_fonte.Text = "Sem Fio";
                        break;
                    case Plugin.Battery.Abstractions.PowerSource.Other:
                        lbl_fonte.Text = "Desconhecido";
                        break;
                }
            } catch (Exception err)
            {
                await DisplayAlert("Ocorreu um erro: \n", err.Message, "OK");
            }
        }

        private async void bnt_on_off_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(!ligada)
                {
                    ligada = true;

                    bnt_on_off.Source = ImageSource.FromResource("app_lanterna.imagens.switch-on.png");
                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));
                    await Flashlight.TurnOnAsync();
                } else
                {
                    ligada = false;

                    bnt_on_off.Source = ImageSource.FromResource("app_lanterna.imagens.switch-off.png");
                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));
                    await Flashlight.TurnOffAsync();
                }
            } catch (Exception err)
            {
                await DisplayAlert("Ocorreu um erro: \n", err.Message, "OK");
            }
        }
    }
}
