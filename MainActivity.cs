using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.Navigation;
using Android.Views.InputMethods;
using Android.Content;
using Android;
using AndroidX.Core.App;
using Android.Bluetooth;

namespace ShishaBacon
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, Icon = "@mipmap/ic_launcher", RoundIcon = "@mipmap/ic_launcher_round")]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private bool finishedInit = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            drawer.DrawerOpened += (sender, e) => { updateUsername(); };
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            //BluetoothHelper.getPermission(BaseContext, this);
            ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, 1);

            TabaccoList.Init(() =>
            {
                RaterSaved.Init(() =>
                {
                    if (RaterSaved.GetRater().Name == Rater.DefaultName)
                    {
                        ShowRaterChange();
                    }
                    else
                    {
                        updateUsername();
                        ShowTabaccoHome();
                    }
                    finishedInit = true;
                });

            });
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                if (FindViewById(Resource.Id.mainFragment).GetType() != typeof(Fragment_Tabacco_list))
                {
                    ShowTabaccoHome();
                }
                else
                {
                    base.OnBackPressed();
                }
            }
        }

        private void updateUsername()
        {
            Rater user = RaterSaved.GetRater();
            if (user != null && FindViewById<AppCompatTextView>(Resource.Id.userName) != null)
                FindViewById<AppCompatTextView>(Resource.Id.userName).SetText(user.Name.ToCharArray(), 0, user.Name.Length);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            if (!finishedInit)
            {
                return false;
            }

            switch (item.ItemId)
            {
                case Resource.Id.nav_tabacco_home:
                    ShowTabaccoHome();
                    break;

                case Resource.Id.nav_tabacco_new:
                    ShowTabaccoNew();
                    break;

                case Resource.Id.nav_user_change:
                    ShowRaterChange();
                    break;

                //case Resource.Id.nav_bluetooth:
                //    ShowSyncBluetooth();
                //    break;

                case Resource.Id.nav_file:
                    ShowSyncFile();
                    break;

                default:
                    break;
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.Selected = false;
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void ShowTabaccoNew()
        {
            Fragment_Tabacco_new f = new Fragment_Tabacco_new();

            f.finished += (sender, e) =>
            {
                ShowTabaccoHome();
            };

            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.mainFragment, f).Commit();
        }

        private void ShowTabaccoHome()
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            if (imm != null && CurrentFocus != null && CurrentFocus.WindowToken != null)
            {
                imm.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
            }
            Fragment_Tabacco_list list = new Fragment_Tabacco_list();

            list.itemClicked += (sender, e) =>
            {
                Fragment_Tabacco_menu f = new Fragment_Tabacco_menu(e.Tabacco);
                f.finished += (sender, e) =>
                {
                    ShowTabaccoHome();
                };
                SupportFragmentManager.BeginTransaction().Replace(Resource.Id.mainFragment, f).Commit();
            };

            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.mainFragment, list).Commit();
        }

        private void ShowRaterChange()
        {
            Fragment_Rater_change f = new Fragment_Rater_change();

            f.finished += (sender, e) =>
            {
                updateUsername();
                ShowTabaccoHome();
            };

            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.mainFragment, f).Commit();
        }

        private void ShowSyncBluetooth()
        {
            Fragment_Sync_Bluetooth f = new Fragment_Sync_Bluetooth();

            f.finished += (sender, e) =>
            {
                ShownBluetoothMenu(e.socket);
            };

            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.mainFragment, f).Commit();
        }

        private void ShownBluetoothMenu(BluetoothSocket socket)
        {
            Fragment_Bluetooth_Menu f = new Fragment_Bluetooth_Menu(socket);

            f.finished += (sender, e) =>
            {
                ShowTabaccoHome();
            };

            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.mainFragment, f).Commit();
        }

        private void ShowSyncFile()
        {
            Fragment_Sync_File f = new Fragment_Sync_File();

            f.finished += (sender, e) =>
            {
                ShowTabaccoHome();
            };

            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.mainFragment, f).Commit();
        }
    }
}


