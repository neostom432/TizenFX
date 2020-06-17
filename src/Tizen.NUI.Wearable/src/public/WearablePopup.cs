/*
 * Copyright (c) 2019 Samsung Electronics Co., Ltd.
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
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace Tizen.NUI.Wearable
{
    public class WearablePopup : Modal
    {
        private TextLabel titleLabel;
        private TextLabel contentLabel;
        private Button cancelButton;
        private Button confirmButton;

        public WearablePopup() : base()
        {
            PositionUsesPivotPoint = true;
            ParentOrigin = Tizen.NUI.ParentOrigin.Center;
            Scale = new Vector3(1.35f,1.35f,1.0f);
            Opacity = 0.0f;
            BackgroundColor = Color.Black;

            Layout = new FlexLayout()
            {
                Direction = FlexLayout.FlexDirection.Row,
                Justification = FlexLayout.FlexJustification.SpaceBetween,
                ItemsAlignment = FlexLayout.AlignmentType.Center,
            };
            Padding = new Extents(3,3,0,0);

            CancelButton = new Button()
            {
                Size = new Size(73,121),
                CornerRadius = 36.5f,
                Text = "",
            };
            CancelButton.ButtonIcon.Size = new Size(50,50);
            CancelButton.ButtonIcon.Color = new Color("#38A4FC");
            CancelButton.Style.BackgroundColor = new ColorSelector()
            {
                Normal = new Color("#002A4DD9"),
                Pressed = new Color("#004680D9"),
            };
            // CancelButton.ButtonIcon.ResourceUrl = "/usr/share/dotnet.tizen/framework/res/" + "nui_component_default_popup_button_delete.png";
            Add(CancelButton);

            ScrollableBase content = new ScrollableBase()
            {
                WidthSpecification = 208,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
            Add(content);

            ConfirmButton = new Button()
            {
                Size = new Size(73,121),
                CornerRadius = 36.5f,
                Text = "",
            };
            ConfirmButton.ButtonIcon.Size = new Size(50,50);
            ConfirmButton.ButtonIcon.Color = new Color("#38A4FC");
            confirmButton.Style.BackgroundColor = new ColorSelector()
            {
                Normal = new Color("#002A4DD9"),
                Pressed = new Color("#004680D9"),
            };
            // ConfirmButton.ButtonIcon.ResourceUrl = "/usr/share/dotnet.tizen/framework/res/" + "nui_component_default_popup_button_check.png";
            Add(ConfirmButton);

            View scrollContainer = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    LinearAlignment = LinearLayout.Alignment.CenterHorizontal,
                },
            };
            content.Add(scrollContainer);

            titleLabel = new TextLabel()
            {
                WidthSpecification = 208,
                HorizontalAlignment = HorizontalAlignment.Center,
                PixelSize = 30,
                TextColor = new Color("#008CFF"),
                Margin = new Extents(0,0,46,5),
            };
            scrollContainer.Add(titleLabel);

            contentLabel = new TextLabel()
            {
                WidthSpecification = 208,
                HeightSpecification = 900,
                HorizontalAlignment = HorizontalAlignment.Center,
                PixelSize = 32,
                TextColor = new Color("#CCCCCC"),
                MultiLine = true,
            };
            scrollContainer.Add(contentLabel);
        }

        public Button ConfirmButton
        {
            get 
            {
                return confirmButton;
            }
            internal set
            {
                confirmButton = value;
            }
        }

        public Button CancelButton
        {
            get 
            {
                return cancelButton;
            }
            internal set
            {
                cancelButton = value;
            }
        }

        public string Title
        {
            get
            {
                return titleLabel.Text;
            }
            set
            {
                titleLabel.Text = value;
            }
        }

        public string Contents
        {
            get
            {
                return contentLabel.Text;
            }
            set
            {
                contentLabel.Text = value;
            }
        }


        protected override void OnPost(Animation animation)
        {
            animation.Duration = 300;
            AlphaFunction ease = new AlphaFunction(new Vector2(0.25f, 0.46f), new Vector2(0.45f, 1f));
            animation.AnimateTo(this, "scale", new Vector3(1.0f, 1.0f, 1.0f), ease);
            animation.AnimateTo(this, "opacity", 1.0f, ease);
            animation.Play();
        }

        protected override void OnDismiss(Animation animation)
        {
            animation.Duration = 300;
            AlphaFunction ease = new AlphaFunction(new Vector2(0.25f, 0.46f), new Vector2(0.45f, 1f));
            animation.AnimateTo(this, "scale", new Vector3(1.35f, 1.35f, 1.0f), ease);
            animation.AnimateTo(this, "opacity", 0.0f, ease);
            animation.Play();
        }
    }
}