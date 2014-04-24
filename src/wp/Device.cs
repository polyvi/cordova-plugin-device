/*  
	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at
	
	http://www.apache.org/licenses/LICENSE-2.0
	
	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Info;
using System.IO.IsolatedStorage;
using System.Windows.Resources;
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;
using xFaceLib.runtime;
using xFaceLib.Util;

namespace WPCordovaClassLib.Cordova.Commands
{
    public class Device : BaseCommand
    {
        public void getDeviceInfo(string notused)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
            string res = String.Format("\"name\":\"{0}\",\"cordova\":\"{1}\",\"platform\":\"{2}\",\"uuid\":\"{3}\",\"version\":\"{4}\",\"model\":\"{5}\"",
                                        this.name,
                                        this.platform,
                                        this.uuid,
                                        this.version,
                                        this.model,
                                        this.xFaceVersion,
                                        this.productVersion,
                                        this.width,
                                        this.height);
                res = "{" + res + "}";
                //Debug.WriteLine("Result::" + res);
                DispatchCommandResult(new PluginResult(PluginResult.Status.OK, res));
            });
        }

        public string model
        {
            get
            {
                return DeviceStatus.DeviceName;
                //return String.Format("{0},{1},{2}", DeviceStatus.DeviceManufacturer, DeviceStatus.DeviceHardwareVersion, DeviceStatus.DeviceFirmwareVersion); 
            }
        }

        public string name
        {
            get
            {
                return DeviceStatus.DeviceName;
                
            }
        }

        public string platform
        {
            get
            {
                return Environment.OSVersion.Platform.ToString();
            }
        }

        public string uuid
        {
            get
            {
                string returnVal = "";
                object id;
                UserExtendedProperties.TryGetValue("ANID", out id);

                if (id != null)
                {
                    returnVal = id.ToString().Substring(2, 32);
                }
                else
                {
                    returnVal = "???unknown???";

                    using (IsolatedStorageFile appStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        try
                        {
                            IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream("DeviceID.txt", FileMode.Open, FileAccess.Read, appStorage);

                            using (StreamReader reader = new StreamReader(fileStream))
                            {
                                returnVal = reader.ReadLine();
                            }
                        }
                        catch (Exception /*ex*/)
                        {

                        }
                    }
                }

                return returnVal;
            }
        }

        public string version
        {
            get
            {
                return Environment.OSVersion.Version.ToString();
            }
        }

        public string xFaceVersion
        {
            get
            {
                string version = XSystemConfiguration.GetInstance().XFaceVersion;
                return version;
            }
        }

        public string productVersion
        {
            get
            {
                //string version = XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;
                //return version;
                return "3.0.0";
            }
        }

        public double width
        {
            get
            {
                switch (XResolutionHelper.CurrentResolution)
                {
                    case Resolutions.HD720p:
                        return 720.0;
                    case Resolutions.WVGA:
                        return 480.0;
                    case Resolutions.WXGA:
                        return 768.0;
                    case Resolutions.HD1080p:
                        return 1080.0;
                    default:
                        throw new InvalidOperationException("Unknown resolution type");
                }
            }
        }

        public double height
        {
            get
            {
                switch (XResolutionHelper.CurrentResolution)
                {
                    case Resolutions.HD720p:
                        return 1280.0;
                    case Resolutions.WVGA:
                        return 800.0;
                    case Resolutions.WXGA:
                        return 1280.0;
                    case Resolutions.HD1080p:
                        return 1920.0;
                    default:
                        throw new InvalidOperationException("Unknown resolution type");
                }
            }
        }
    }
}
