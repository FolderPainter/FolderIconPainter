using Blazored.LocalStorage;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor;
using MudBlazor.Services;
using System.Linq;
using WebApp.Services;
using WebApp.Services.UserPreferences;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 10000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });
            services.AddSignalR(e => {
                e.MaximumReceiveMessageSize = 102400000;
            });
            services.AddBlazoredLocalStorage();
            services.AddScoped<IUserPreferencesService, UserPreferencesService>();
            services.AddScoped<LayoutService>();
            services.AddScoped<IconService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });


            if (HybridSupport.IsElectronActive)
            {
                CreateWindow();
            }
        }

        private async void CreateWindow()
        {
            var window = await Electron.WindowManager.CreateWindowAsync(
                new BrowserWindowOptions
                {
                    Show= false,
                    MinHeight = 600,
                    MinWidth = 400,

                    WebPreferences = new WebPreferences
                    {
                        ContextIsolation = false
                    }
                });

            if (HybridSupport.IsElectronActive)
            {
                Electron.App.Ready += () => CreateContextMenu();

                var menu = new MenuItem[] {
                new MenuItem { Label = "Edit", Submenu = new MenuItem[] {
                    new MenuItem { Label = "Undo", Accelerator = "CmdOrCtrl+Z", Role = MenuRole.undo },
                    new MenuItem { Label = "Redo", Accelerator = "Shift+CmdOrCtrl+Z", Role = MenuRole.redo },
                    new MenuItem { Type = MenuType.separator },
                    new MenuItem { Label = "Cut", Accelerator = "CmdOrCtrl+X", Role = MenuRole.cut },
                    new MenuItem { Label = "Copy", Accelerator = "CmdOrCtrl+C", Role = MenuRole.copy },
                    new MenuItem { Label = "Paste", Accelerator = "CmdOrCtrl+V", Role = MenuRole.paste },
                    new MenuItem { Label = "Select All", Accelerator = "CmdOrCtrl+A", Role = MenuRole.selectall }
                }
                },
                new MenuItem { Label = "View", Submenu = new MenuItem[] {
                    new MenuItem
                    {
                        Label = "Reload",
                        Accelerator = "CmdOrCtrl+R",
                        Click = () =>
                        {
                            // on reload, start fresh and close any old
                            // open secondary windows
                            var mainWindowId = window.Id;
                            Electron.WindowManager.BrowserWindows.ToList().ForEach(browserWindow => {
                                if(browserWindow.Id != mainWindowId)
                                {
                                    browserWindow.Close();
                                }
                                else
                                {
                                    browserWindow.Reload();
                                }
                            });
                        }
                    },
                    new MenuItem
                    {
                        Label = "Toggle Full Screen",
                        Accelerator = "CmdOrCtrl+F",
                        Click = async () =>
                        {
                            bool isFullScreen = await window.IsFullScreenAsync();
                            window.SetFullScreen(!isFullScreen);
                        }
                    },
                    new MenuItem
                    {
                        Label = "Open Developer Tools",
                        Accelerator = "CmdOrCtrl+I",
                        Click = () => window.WebContents.OpenDevTools()
                    },
                    new MenuItem
                    {
                        Type = MenuType.separator
                    }
                }
                },
                new MenuItem { Label = "Window", Role = MenuRole.window, Submenu = new MenuItem[] {
                     new MenuItem { Label = "Minimize", Accelerator = "CmdOrCtrl+M", Role = MenuRole.minimize },
                     new MenuItem { Label = "Close", Accelerator = "CmdOrCtrl+W", Role = MenuRole.close }
                }
                },
                new MenuItem { Label = "Help", Role = MenuRole.help, Submenu = new MenuItem[] {
                    new MenuItem
                    {
                        Label = "Learn More",
                        Click = async () => await Electron.Shell.OpenExternalAsync("https://github.com/ElectronNET")
                    }
                }
                }
            };

                Electron.Menu.SetApplicationMenu(menu);

            }


            //await window.WebContents.Session.ClearCacheAsync();
            window.OnReadyToShow += () => window.Show();
            //window.OnClosed += () =>
            //{
            //    Electron.App.Quit();
            //};
        }

        private void CreateContextMenu()
        {
            var menu = new MenuItem[]
            {
                new MenuItem
                {
                    Label = "Hello",
                    Click = async () => await Electron.Dialog.ShowMessageBoxAsync("Electron.NET rocks!")
                },
                new MenuItem { Type = MenuType.separator },
                new MenuItem { Label = "Electron.NET", Type = MenuType.checkbox, Checked = true }
            };

            var mainWindow = Electron.WindowManager.BrowserWindows.FirstOrDefault();
            Electron.Menu.SetContextMenu(mainWindow, menu);

            Electron.IpcMain.On("show-context-menu", (args) =>
            {
                var mainWindow = Electron.WindowManager.BrowserWindows.FirstOrDefault();
                Electron.Menu.ContextMenuPopup(mainWindow);
            });
        }
    }
}
