/*
 * Copyright(c) 2020 Samsung Electronics Co., Ltd.
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
using System;
using System.Collections.Generic;
using Tizen.NUI.BaseComponents;
using System.ComponentModel;

namespace Tizen.NUI.Components
{
    /// <summary>
    /// Tab is one kind of common component, it can be used as menu label.
    /// User can handle Tab by adding/inserting/deleting TabItem.
    /// </summary>
    /// <since_tizen> 6 </since_tizen>
    public class Modal : Control
    {

        private Window targetWindow;
        private Layer modalLayer;
        private Animation postAnimation;
        private Animation dismissAnimation;

 
        /// <summary>
        /// Creates a new instance of a Modal.
        /// </summary>
        public Modal() : base()
        {
            Initialize();
        }

        private void Initialize()
        {
            modalLayer = new Layer();
            modalLayer.SetTouchConsumed(true);
            modalLayer.Add(this);

            postAnimation = new Animation(0);
            dismissAnimation = new Animation(0);
        }

        public void Post(Window window)
        {
            targetWindow = window;
            Size = targetWindow.WindowSize;

            targetWindow.AddLayer(modalLayer);
            modalLayer.RaiseToTop();

            dismissAnimation.Stop();
            postAnimation.Reset();

            OnPost(postAnimation);
        }

        protected virtual void OnPost(Animation animation)
        {
            animation.Duration = 0;
            Position = new Position(0, 0);
            animation.Play();
        }

        public void Dismiss()
        {
            postAnimation.Stop();
            dismissAnimation.Reset();

            OnDismiss(dismissAnimation);

            dismissAnimation.Finished += (object source, EventArgs args) =>
            {
                targetWindow?.RemoveLayer(modalLayer);
            };
        }

        protected virtual void OnDismiss(Animation animation)
        {
            animation.Duration = 0;
            animation.Play();
        }
    }
}
