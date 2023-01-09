using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;
namespace Blackjack_C._1_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Gebruikte variabelen

        private Random rndKaart = new Random();
        private int aantalKerenGespeeld = 0;
        private bool erIsGedruktOpStandKnop = false;

        private const bool isSpeler = true;
        private const double inzetSpelerStart = 100.00;
        private double kapitaal;
        private double inzet;
        private double gewonnenBedragS;
        private double minimumBedrag;
        private int kaartAantalS = 0;
        private int kaartwaardeS = 0;
        private int totaalKaartwaardeS = 0;
        private bool spelerHeeftOoitAasGekregen = false;
        private StringBuilder sbSpeler = new StringBuilder();

        private int kaartAantalB = 0;
        private int kaartwaardeB = 0;
        private int totaalKaartwaardeB = 0;
        private bool bankHeeftOoitAasGekregen = false;
        private StringBuilder sbBank = new StringBuilder();
        #endregion

        #region DispatcherTimer
        private DispatcherTimer klok;
        private void Klok_Tick(object sender, EventArgs e)
        {
            LblKlok.Content = DateTime.Now.ToLongTimeString();
        }
        #endregion

        #region Deck Kaarten
        /// <summary>
        /// Hier zijn alle gegevens van de kaarten in een 2D array.
        /// </summary>
        private string[,] kaarten = new string[52, 3]
        {
            {"Aas", "Harten", "11"},
            {"2", "Harten", "2"},
            {"3", "Harten", "3"},
            {"4", "Harten", "4"},
            {"5", "Harten", "5"},
            {"6", "Harten", "6"},
            {"7", "Harten", "7"},
            {"8", "Harten", "8"},
            {"9", "Harten", "9"},
            {"10", "Harten", "10"},
            {"Boer", "Harten", "10"},
            {"Dame", "Harten", "10"},
            {"Heer", "Harten", "10"},
            {"Aas", "Ruiten", "11"},
            {"2", "Ruiten", "2"},
            {"3", "Ruiten", "3"},
            {"4", "Ruiten", "4"},
            {"5", "Ruiten", "5"},
            {"6", "Ruiten", "6"},
            {"7", "Ruiten", "7"},
            {"8", "Ruiten", "8"},
            {"9", "Ruiten", "9"},
            {"10", "Ruiten", "10"},
            {"Boer", "Ruiten", "10"},
            {"Dame", "Ruiten", "10"},
            {"Heer", "Ruiten", "10"},
            {"Aas", "Klaver", "11"},
            {"2", "Klaver", "2"},
            {"3", "Klaver", "3"},
            {"4", "Klaver", "4"},
            {"5", "Klaver", "5"},
            {"6", "Klaver", "6"},
            {"7", "Klaver", "7"},
            {"8", "Klaver", "8"},
            {"9", "Klaver", "9"},
            {"10", "Klaver", "10"},
            {"Boer", "Klaver", "10"},
            {"Dame", "Klaver", "10"},
            {"Heer", "Klaver", "10"},
            {"Aas", "Schoppen", "11"},
            {"2", "Schoppen", "2"},
            {"3", "Schoppen", "3"},
            {"4", "Schoppen", "4"},
            {"5", "Schoppen", "5"},
            {"6", "Schoppen", "6"},
            {"7", "Schoppen", "7"},
            {"8", "Schoppen", "8"},
            {"9", "Schoppen", "9"},
            {"10", "Schoppen", "10"},
            {"Boer", "Schoppen", "10"},
            {"Dame", "Schoppen", "10"},
            {"Heer", "Schoppen", "10"}
        };
        #endregion

        #region Methode NieuwSpel
        /// <summary>
        /// Deze methode zorgt voor een volledig nieuwe start en hernieuwt de kapitaal naar de originele waarde.
        /// Ook worden bepaalde knoppen aan of uitgezet.
        /// </summary>
        private void NieuwSpel()
        {
            BtnNieuwSpel.Visibility = Visibility.Hidden;
            BtnNieuwSpel.IsEnabled = false;
            SldInzetS.IsEnabled = true;
            BtnDeel.IsEnabled = true;
            kapitaal = inzetSpelerStart;
            LblGeldS.Content = kapitaal;
            minimumBedrag = Math.Ceiling(kapitaal / 10);

            SldInzetS.Minimum = minimumBedrag;
            SldInzetS.Maximum = kapitaal;
            SldInzetS.Value = minimumBedrag;
        }
        #endregion

        #region Methode BeginRonde
        /// <summary>
        /// Deze methode bevat alle acties die ondernomen moeten worden aan het begin van de ronde.
        /// 
        /// Deze zijn geroepeerd als volgt:
        /// - Inzet en kapitaal gerelateerde variablen.
        /// - Speler gerelateerde variabelen.
        /// - Bank gerelateerde variabelen
        /// - Algemene variabelen
        /// </summary>
        private void BeginRonde()
        {
            kapitaal -= inzet;
            LblGeldS.Content = kapitaal;
            gewonnenBedragS = 0;
            SldInzetS.IsEnabled = false;
            LblInzetS.Foreground = new SolidColorBrush(Colors.White);

            kaartAantalS = 0;
            kaartwaardeS = 0;
            totaalKaartwaardeS = 0;
            spelerHeeftOoitAasGekregen = true;
            sbSpeler = new StringBuilder();
            TxtResultSpeler.Clear();
            LblSpelerPunten.Content = "0";
            ImgKaartS.Visibility = Visibility.Hidden;

            kaartwaardeB = 0;
            totaalKaartwaardeB = 0;
            kaartAantalB = 0;
            bankHeeftOoitAasGekregen = true;
            sbBank= new StringBuilder();
            TxtResultBank.Clear();
            LblBankPunten.Content = "0";
            ImgKaartB.Visibility = Visibility.Hidden;

            LblResultaatGame.Visibility = Visibility.Hidden;
            BorderResultaat.Visibility = Visibility.Hidden;
            erIsGedruktOpStandKnop = false;
            BtnDeel.IsEnabled = false;                                                      // Deel-knop wordt uitgezet totdat er een push/win/loss is
            BtnHit.IsEnabled = true;                                                        // Hit-knop optie wordt beschikbaar nadat kaarten gedeeld zijn
            BtnStand.IsEnabled = true;                                                      // Stand-knop optie wordt beschikbaar nadat kaarten gedeeld zijn
            if (inzet * 2 < kapitaal)
            {
                BtnDouble.IsEnabled = true;
            }
            
        }
        #endregion

        #region Methode EindeRonde
        /// <summary>
        /// Deze methode bevat alle acties die ondernomen moeten worden aan het einde van de ronde.
        /// Voorbeelden:
        /// - Het zichtbaar maken van verborgen kaarten en nog niet opgetelde punten.
        /// - Zichtbaar maken of verbergen van bepaalde buttons.
        /// - Optellen van aantal gespeelde spellen.
        /// </summary>
        private void EindeRonde()
        {
            ResterendePuntenEnKaartenZichtbaarMaken();
            aantalKerenGespeeld++;
            LblAantalKerenGespeeld.Content= "Aantal keren gespeeld: " + aantalKerenGespeeld;

            kapitaal += gewonnenBedragS;
            minimumBedrag = Math.Ceiling(kapitaal / 10);
            LblGeldS.Content = kapitaal;
            SldInzetS.Maximum = kapitaal;
            SldInzetS.Minimum = minimumBedrag;
            SldInzetS.TickFrequency = kapitaal/10;

            BtnHit.IsEnabled = false;
            BtnStand.IsEnabled = false;
            SldInzetS.IsEnabled = true;

            if (kapitaal == 0)
            {
                BtnNieuwSpel.IsEnabled = true;
                BtnNieuwSpel.Visibility= Visibility.Visible;
                MessageBox.Show("U heeft al uw geld verloren!\nGeen zorgen, je krijgt een nieuwe kans.", "Helaas!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                BtnDeel.IsEnabled = true;
            }
        }
        #endregion

        #region Methode ResterendePuntenEnKaartenZichtbaarMaken
        /// <summary>
        /// Deze methode heeft als opdracht om de verborgen 2de kaart van de bank ook zichtbaar te maken.
        /// </summary>
        private void ResterendePuntenEnKaartenZichtbaarMaken()
        {
            LblResultaatGame.Visibility = Visibility.Visible;
            BorderResultaat.Visibility = Visibility.Visible;
            if (kaartAantalB == 2)
            {
                TxtResultBank.Text = sbBank.ToString();
                LblBankPunten.Content = Convert.ToString(totaalKaartwaardeB);
            }
        }
        #endregion

        #region Methodes Gewonnen, Verloren en Gelijkspel
        /// <summary>
        /// Hier zijn de 3 methodes van 3 mogelijke spelresultaten.
        /// </summary>
        /// <param name="isBlackJack"></param>
        private void Gewonnen(bool isBlackJack)
        {
            LblInzetS.Foreground = new SolidColorBrush(Colors.Green);
            LblResultaatGame.Foreground = new SolidColorBrush(Colors.Green);
            BorderResultaat.Background = new SolidColorBrush(Colors.Black);
            if (isBlackJack)
            {
                gewonnenBedragS = inzet * 3;
                LblLaatsteSpel.Content = "Resultaat vorige spel: - Gewonnen bedrag: " + gewonnenBedragS + "(Kapitaal: " + kapitaal+gewonnenBedragS +")";
                EindeRonde();
                LblResultaatGame.Content = "Gewonnen!";
            }
            else
            {
                gewonnenBedragS = inzet * 2;
                LblLaatsteSpel.Content = "Resultaat vorige spel: - Gewonnen bedrag: " + gewonnenBedragS + "(Kapitaal: " + kapitaal + gewonnenBedragS + ")";
                EindeRonde();
                LblResultaatGame.Content = "Gewonnen!";
            }
        }
        private void Verloren()
        {
            LblLaatsteSpel.Content = "Resultaat vorige spel: - Verloren bedrag: " + inzet + "(Kapitaal: " + kapitaal + ")";
            gewonnenBedragS = 0;
            LblInzetS.Foreground = new SolidColorBrush(Colors.Red);
            EindeRonde();
            LblResultaatGame.Foreground = new SolidColorBrush(Colors.Red);
            BorderResultaat.Background = new SolidColorBrush(Colors.Black);
            LblResultaatGame.Content = "Verloren!";
        }
        private void Gelijkspel()
        {
            LblLaatsteSpel.Content = "Resultaat vorige spel: - Teruggekregen bedrag: " + inzet + "(Kapitaal: " + kapitaal + inzet + ")";
            gewonnenBedragS = inzet;
            LblInzetS.Foreground = new SolidColorBrush(Colors.Green);
            EindeRonde();
            LblResultaatGame.Foreground = new SolidColorBrush(Colors.Black);
            BorderResultaat.Background = new SolidColorBrush(Colors.DarkGray);
            LblResultaatGame.Content = "Push!";
        }
        #endregion

        #region Methode NieuwKaart
        /// <summary>
        /// Hier Neemt men telkens een nieuwe string van de kaartendeck adhv een gegenereerde random nummer "arrayCode" 
        /// Deze methode had als bedoeling om nieuwe kaart te genereren uit een bestaande kaartendeck maar heb niet genoeg tijd gehad om het te kunnen laten werken dus kan het helaas ook dubbele kaarten genereren.
        /// </summary>
        /// <returns></returns>
        private string[] NieuwKaart()
        {

            int arrayCode = rndKaart.Next(52);                                              // Creëert random nummer > 52 om kaart te selecteren uit deck
            string[] nieuwKaart = { kaarten[arrayCode, 0], kaarten[arrayCode, 1], kaarten[arrayCode, 2] };
            return nieuwKaart;
        }
        #endregion

        #region Methode GeefKaart
        /// <summary>
        /// De GeefKaart methode bevat bijna al de acties die uitgevoerd moeten worden zodra er een kaart wordt gevraagd.
        /// Voorbeeld:
        /// - Het genereren van de kaart voor de juiste persoon (Speler of Bank)
        /// - Het aantal keren generen van de kaarten.
        /// - Het updaten van de punten en textvelden met de namen van de kaarten.
        /// - Het beslissen van de waarden van de eventuele azen.
        /// - Het nakijken vande punten en winnaars/verliezers/gelijkspelers bepalen. 
        /// </summary>
        /// <param name="isSpeler"></param>
        /// Deze parameter helpt bepalen wie en hoeveel kaarten men krijgt.
        private async void GeefKaart(bool isSpeler)                                         // De methode die kaarten genereert/teruggeeft adhv gegeven parameter "isSpeler"
        {
            await Task.Delay(1000);
            if (isSpeler)                                                                   //=SPELER=============================================================
            {
                BtnDouble.IsEnabled = false;                                                                      
                do                                                                          // SPELER verkrijgt 1 kaart, tenzij deze de eerste twee kaarten moeten zijn (Bij het "delen")
                {
                    
                    string[] randomKaart = NieuwKaart();

                    ImgKaartS.Visibility = Visibility.Visible;
                    ImgKaartS.Source = new BitmapImage(new Uri("/Resources/cards/" + randomKaart[0] + "_" + randomKaart[1] + ".png", UriKind.Relative));

                    sbSpeler.AppendLine(randomKaart[1] + " " + randomKaart[0]);             // Voegt de kaartnaam toe aan stringbuilder van de speler
                    TxtResultSpeler.Text = sbSpeler.ToString();                             // Kaartnaam van speler wordt zichtbaar gemaakt


                    kaartwaardeS = int.Parse(randomKaart[2]);                               // Hier controleert men voor dubbele azen en de punten worden zodanig aangepast
                    if (kaartwaardeS == 11)
                    {
                        if (totaalKaartwaardeS + kaartwaardeS > 21)
                        {
                            kaartwaardeS = 1;
                        }
                        else
                        {
                            spelerHeeftOoitAasGekregen = true;
                        }
                    }
                    else if (spelerHeeftOoitAasGekregen == true && totaalKaartwaardeS > 21)
                    {
                        spelerHeeftOoitAasGekregen = false;
                        totaalKaartwaardeS -= 10;
                    }

                    totaalKaartwaardeS += kaartwaardeS;
                    LblSpelerPunten.Content = Convert.ToString(totaalKaartwaardeS);
                    kaartAantalS++;
                    
                } while (kaartAantalS < 2);

            }
            else                                                                            //=BANK===================================================================
            {
                do
                {
                    string[] randomKaart = NieuwKaart();
                    kaartAantalB++;
                    ImgKaartB.Visibility = Visibility.Visible;

                    if (kaartAantalB != 1)
                    {
                    ImgKaartB.Source = new BitmapImage(new Uri("/Resources/cards/" + randomKaart[0] + "_" + randomKaart[1] + ".png", UriKind.Relative));
                    }

                    switch (kaartAantalB)
                    {
                        case 1:
                            sbBank.AppendLine(randomKaart[1] + " " + randomKaart[0]);
                            TxtResultBank.Text = sbBank.ToString();
                            break;
                        case 2:
                            sbBank.AppendLine(randomKaart[1] + " " + randomKaart[0]);
                            break;
                        case 3:
                            TxtResultBank.Text = sbBank.ToString();
                            sbBank.AppendLine(randomKaart[1] + " " + randomKaart[0]);
                            TxtResultBank.Text = sbBank.ToString();
                            break;
                        default:
                            sbBank.AppendLine(randomKaart[1] + " " + randomKaart[0]);
                            TxtResultBank.Text = sbBank.ToString();
                            break;
                    }
                    kaartwaardeB = int.Parse(randomKaart[2]);

                    if (kaartwaardeB == 11)
                    {
                        if (totaalKaartwaardeB + kaartwaardeB > 21)
                        {
                            kaartwaardeB = 1;
                        }
                        else
                        {
                            bankHeeftOoitAasGekregen = true;  
                        }
                    }
                    else if (bankHeeftOoitAasGekregen == true && totaalKaartwaardeB > 21)
                    {
                        bankHeeftOoitAasGekregen = false;
                        totaalKaartwaardeB -= 10;
                    }
                    totaalKaartwaardeB += kaartwaardeB;

                    if (kaartAantalB == 1)
                    {
                        LblBankPunten.Content = Convert.ToString(totaalKaartwaardeB);
                    }
                    else if (kaartAantalB == 2)
                    {

                    }
                    else
                    {
                        LblBankPunten.Content = Convert.ToString(totaalKaartwaardeB);
                    }
                    

                } while (totaalKaartwaardeB < 17 && totaalKaartwaardeB < 21 && erIsGedruktOpStandKnop == true);
            }
            //==============================================================================//Controle resultaat

            if (kaartAantalS == 2 && kaartAantalB == 2)                                     // na Deel-knop
            {
                if (totaalKaartwaardeS == 21)
                {
                    BtnHit.IsEnabled = false;
                }
            }
            else if (kaartAantalS >= 2  && totaalKaartwaardeS > 21)                         // na Hit-knop
            {
                Verloren();
            }
            else if (kaartAantalS >= 2 && totaalKaartwaardeB > 21)
            {
                Gewonnen(false);
            }
            else if ((kaartAantalS >= 2 || kaartAantalB >= 2) && totaalKaartwaardeB > 16 && erIsGedruktOpStandKnop == true)   // na Stand-knop
            {
                BtnHit.IsEnabled = false;
                
                if (totaalKaartwaardeS == 21)
                {
                    if (totaalKaartwaardeB == 21)
                    {
                        Gelijkspel();
                    }
                    else if (totaalKaartwaardeB > 21)
                    {
                        Gewonnen(true);
                    }
                    else
                    {
                        GeefKaart(false);
                    }
                }
                else if (totaalKaartwaardeS > 21)
                {
                    Verloren();
                }
                else if (totaalKaartwaardeB > 21)
                {
                    Gewonnen(false);
                }
                else if (totaalKaartwaardeS == totaalKaartwaardeB)
                {
                    Gelijkspel();
                }
                else if (totaalKaartwaardeS > totaalKaartwaardeB)
                {
                    if (bankHeeftOoitAasGekregen == true)
                    {
                        GeefKaart(false);
                    }
                    else
                    {
                    Gewonnen(false);
                    }
                }
                else
                {
                    Verloren();
                }
            }
        }
        #endregion

//========================================================================================
//========================================================================================
//========================================================================================
//==============START PROGRAMMA===========================================================
        public MainWindow()
        {
            InitializeComponent();
            SldInzetS.IsEnabled = false;
            BtnDeel.IsEnabled= false;
            BtnHit.IsEnabled = false;
            BtnStand.IsEnabled = false;
            BtnDouble.IsEnabled = false;
            
            klok = new DispatcherTimer();
            klok.Tick += Klok_Tick;

            klok.Start();

        }

//========================================================================================
//===============START NIEUW SPEL=========================================================


        #region Knoppen Nieuwspel Deel, Hit, Stand, Deel en Slider Inzet
        private void BtnNieuwSpel_Click(object sender, RoutedEventArgs e)
        {
            //NIEUW SPEL=======
            NieuwSpel();
        }
        private void BtnDeel_Click(object sender, RoutedEventArgs e)
        {
            //NIEUW RONDE======
            BeginRonde();

            //SPELER=======
            GeefKaart(true);
            //BANK=======
            GeefKaart(false);

        }

        private void BtnHit_Click(object sender, RoutedEventArgs e)
        {
            //SPELER HIT=======
            GeefKaart(true);
        }

        private void BtnStand_Click(object sender, RoutedEventArgs e)
        {
            //BANK STAND=======
            erIsGedruktOpStandKnop = true;
            GeefKaart(false);
        }

        private void BtnDouble_Click(object sender, RoutedEventArgs e)
        {
            //SPELER HIT=======
            kapitaal -= inzet;
            LblGeldS.Content= kapitaal;
            inzet *= 2;
            LblInzetS.Content = inzet;

        }
        private void SldInzetS_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LblInzetS.Content = SldInzetS.Value;
            inzet = Convert.ToDouble(LblInzetS.Content);
            LblInzetS.Foreground = new SolidColorBrush(Colors.White);
            ImgKaartS.Visibility = Visibility.Hidden;
            ImgKaartB.Visibility = Visibility.Hidden;
        }
        #endregion
    }
}
