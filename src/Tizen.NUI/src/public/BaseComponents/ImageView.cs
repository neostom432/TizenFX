/** Copyright (c) 2017 Samsung Electronics Co., Ltd.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

namespace Tizen.NUI.BaseComponents
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// ImageView is a class for displaying an image resource.<br>
    /// An instance of ImageView can be created using a URL or an Image instance.<br>
    /// </summary>
    public class ImageView : View
    {
        private global::System.Runtime.InteropServices.HandleRef swigCPtr;

        internal ImageView(global::System.IntPtr cPtr, bool cMemoryOwn) : base(NDalicPINVOKE.ImageView_SWIGUpcast(cPtr), cMemoryOwn)
        {
            swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
        }

        internal static global::System.Runtime.InteropServices.HandleRef getCPtr(ImageView obj)
        {
            return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
        }


        /// <summary>
        /// Event arguments of resource ready.
        /// </summary>
        public class ResourceReadyEventArgs : EventArgs
        {
            private View _view;

            /// <summary>
            /// The view whose resource is ready.
            /// </summary>
            public View View
            {
                get
                {
                    return _view;
                }
                set
                {
                    _view = value;
                }
            }
        }

        private EventHandler<ResourceReadyEventArgs> _resourceReadyEventHandler;
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void ResourceReadyEventCallbackType(IntPtr data);
        private ResourceReadyEventCallbackType _resourceReadyEventCallback;

        /// <summary>
        /// Event for ResourceReady signal which can be used to subscribe/unsubscribe the event handler.<br>
        /// This signal is emitted after all resources required by a control are loaded and ready.<br>
        /// Most resources are only loaded when the control is placed on stage.<br>
        /// </summary>
        public event EventHandler<ResourceReadyEventArgs> ResourceReady
        {
            add
            {
                if (_resourceReadyEventHandler == null)
                {
                    _resourceReadyEventCallback = OnResourceReady;
                    ResourceReadySignal(this).Connect(_resourceReadyEventCallback);
                }

                _resourceReadyEventHandler += value;
            }

            remove
            {
                _resourceReadyEventHandler -= value;

                if (_resourceReadyEventHandler == null && ResourceReadySignal(this).Empty() == false)
                {
                    ResourceReadySignal(this).Disconnect(_resourceReadyEventCallback);
                }
            }
        }

        // Callback for View ResourceReady signal
        private void OnResourceReady(IntPtr data)
        {
            ResourceReadyEventArgs e = new ResourceReadyEventArgs();
            if(data != null)
            {
                e.View = Registry.GetManagedBaseHandleFromNativePtr(data) as View;
            }

            if (_resourceReadyEventHandler != null)
            {
                _resourceReadyEventHandler(this, e);
            }
        }

        //you can override it to clean-up your own resources.
        protected override void Dispose(DisposeTypes type)
        {
            if (disposed)
            {
                return;
            }

            if(type == DisposeTypes.Explicit)
            {
                //Called by User
                //Release your own managed resources here.
                //You should release all of your own disposable objects here.
                _border?.Dispose();
                _border = null;
                _nPatchMap?.Dispose();
                _nPatchMap = null;
            }

            //Release your own unmanaged resources here.
            //You should not access any managed member here except static instance.
            //because the execution order of Finalizes is non-deterministic.

            if (swigCPtr.Handle != global::System.IntPtr.Zero)
            {
                if (swigCMemOwn)
                {
                    swigCMemOwn = false;
                    NDalicPINVOKE.delete_ImageView(swigCPtr);
                }
                swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
            }

            base.Dispose(type);
        }

        internal class Property
        {
            internal static readonly int RESOURCE_URL = NDalicPINVOKE.ImageView_Property_RESOURCE_URL_get();
            internal static readonly int IMAGE = NDalicPINVOKE.ImageView_Property_IMAGE_get();
            internal static readonly int PRE_MULTIPLIED_ALPHA = NDalicPINVOKE.ImageView_Property_PRE_MULTIPLIED_ALPHA_get();
            internal static readonly int PIXEL_AREA = NDalicPINVOKE.ImageView_Property_PIXEL_AREA_get();
        }

        /// <summary>
        /// Creates an initialized ImageView.
        /// </summary>
        public ImageView() : this(NDalicPINVOKE.ImageView_New__SWIG_0(), true)
        {
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

        }
        /// <summary>
        /// Creates an initialized ImageView from an URL to an image resource.<br>
        /// If the string is empty, ImageView will not display anything.<br>
        /// </summary>
        /// <param name="url">The url of the image resource to display</param>
        public ImageView(string url) : this(NDalicPINVOKE.ImageView_New__SWIG_2(url), true)
        {
            _url = url;
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

        }
        internal ImageView(string url, Uint16Pair size) : this(NDalicPINVOKE.ImageView_New__SWIG_3(url, Uint16Pair.getCPtr(size)), true)
        {
            _url = url;
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();

        }

        [Obsolete("Please do not use! this will be deprecated")]
        public new static ImageView DownCast(BaseHandle handle)
        {
            ImageView ret =  Registry.GetManagedBaseHandleFromNativePtr(handle) as ImageView;
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }
        /// <summary>
        /// Sets this ImageView from the given URL.<br>
        /// If the URL is empty, ImageView will not display anything.<br>
        /// </summary>
        /// <param name="url">The URL to the image resource to display</param>
        public void SetImage(string url)
        {
            _url = url;
            NDalicPINVOKE.ImageView_SetImage__SWIG_1(swigCPtr, url);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }
        internal void SetImage(string url, Uint16Pair size)
        {
            _url = url;
            NDalicPINVOKE.ImageView_SetImage__SWIG_2(swigCPtr, url, Uint16Pair.getCPtr(size));
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        internal ViewResourceReadySignal ResourceReadySignal(View view) {
            ViewResourceReadySignal ret = new ViewResourceReadySignal(NDalicPINVOKE.ResourceReadySignal(View.getCPtr(view)), false);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// Query if all resources required by a control are loaded and ready.<br>
        /// Most resources are only loaded when the control is placed on stage.<br>
        /// true if the resources are loaded and ready, false otherwise.<br>
        /// </summary>
        public bool IsResourceReady()
        {
            bool ret = NDalicPINVOKE.IsResourceReady(swigCPtr);
            if (NDalicPINVOKE.SWIGPendingException.Pending)
                throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// <summary>
        /// ImageView ResourceUrl, type string
        /// </summary>
        public string ResourceUrl
        {
            get
            {
                GetProperty(ImageView.Property.RESOURCE_URL).Get(out _url);
                return _url;
            }
            set
            {
                _url = value;
                UpdateImage();
            }
        }

        /// <summary>
        /// ImageView ImageMap, type PropertyMap : string if it is a url, map otherwise
        /// </summary>
        public PropertyMap ImageMap
        {
            get
            {
                if (_border == null)
                {
                    PropertyMap temp = new PropertyMap();
                    GetProperty(ImageView.Property.IMAGE).Get(temp);
                    return temp;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (_border == null)
                {
                    SetProperty(ImageView.Property.IMAGE, new Tizen.NUI.PropertyValue(value));
                }
            }
        }

        /// <summary>
        /// ImageView PreMultipliedAlpha, type Boolean.<br>
        /// Image must be initialized.<br>
        /// </summary>
        public bool PreMultipliedAlpha
        {
            get
            {
                bool temp = false;
                GetProperty(ImageView.Property.PRE_MULTIPLIED_ALPHA).Get(out temp);
                return temp;
            }
            set
            {
                SetProperty(ImageView.Property.PRE_MULTIPLIED_ALPHA, new Tizen.NUI.PropertyValue(value));
            }
        }

        /// <summary>
        /// ImageView PixelArea, type Vector4 (Animatable property).<br>
        /// Pixel area is a relative value with the whole image area as [0.0, 0.0, 1.0, 1.0].<br>
        /// </summary>
        public RelativeVector4 PixelArea
        {
            get
            {
                Vector4 temp = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
                GetProperty(ImageView.Property.PIXEL_AREA).Get(temp);
                return temp;
            }
            set
            {
                SetProperty(ImageView.Property.PIXEL_AREA, new Tizen.NUI.PropertyValue(value));
            }
        }

        /// <summary>
        /// The border of the image in the order: left, right, bottom, top.<br>
        /// If set, ImageMap will be ignored.<br>
        /// For N-Patch images only.<br>
        /// Optional.
        /// </summary>
        public Rectangle Border
        {
            get
            {
                return _border;
            }
            set
            {
                _border = value;
                UpdateImage();
            }
        }

        /// <summary>
        /// Get or set whether to draws the borders only(If true).<br>
        /// If not specified, the default is false.<br>
        /// For N-Patch images only.<br>
        /// Optional.
        /// </summary>
        public bool BorderOnly
        {
            get
            {
                return _borderOnly ?? false;
            }
            set
            {
                _borderOnly = value;
                UpdateImage();
            }
        }

        public bool SynchronosLoading
        {
            get
            {
                return _synchronousLoading ?? false;
            }
            set
            {
                _synchronousLoading = value;
                UpdateImage();
            }
        }

        private void UpdateImage()
        {
            if (_url != null)
            {
                if (_border != null)
                { // for nine-patch image
                    _nPatchMap = new PropertyMap();
                    _nPatchMap.Add(Visual.Property.Type, new PropertyValue((int)Visual.Type.NPatch));
                    _nPatchMap.Add(NpatchImageVisualProperty.URL, new PropertyValue(_url));
                    _nPatchMap.Add(NpatchImageVisualProperty.Border, new PropertyValue(_border));
                    if (_borderOnly != null) { _nPatchMap.Add(NpatchImageVisualProperty.BorderOnly, new PropertyValue((bool)_borderOnly)); }
                    if (_synchronousLoading != null) _nPatchMap.Add(NpatchImageVisualProperty.SynchronousLoading, new PropertyValue((bool)_synchronousLoading));
                    SetProperty(ImageView.Property.IMAGE, new PropertyValue(_nPatchMap));
                }
                else if(_synchronousLoading != null)
                { // for normal image, with synchronous loading property
                    PropertyMap imageMap = new PropertyMap();
                    imageMap.Add(Visual.Property.Type, new PropertyValue((int)Visual.Type.Image));
                    imageMap.Add(ImageVisualProperty.URL, new PropertyValue(_url));
                    imageMap.Add(ImageVisualProperty.SynchronousLoading, new PropertyValue((bool)_synchronousLoading));
                    SetProperty(ImageView.Property.IMAGE, new PropertyValue(imageMap));
                }
                else
                { // just for normal image
                    SetProperty(ImageView.Property.RESOURCE_URL, new PropertyValue(_url));
                }
            }
        }

        private Rectangle _border = null;
        private PropertyMap _nPatchMap = null;
        private bool? _synchronousLoading = null;
        private bool? _borderOnly = null;
        private string _url = null;

    }

}