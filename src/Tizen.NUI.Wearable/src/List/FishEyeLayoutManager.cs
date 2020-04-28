using System;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

using System.Collections.Generic;
using System.ComponentModel;


namespace Tizen.NUI.Wearable
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class FishEyeLayoutManager : LayoutManager
    {
        public int CurrentFocusedIndex{get;set;} = 0;
        public int FocusedIndex{get;set;} = 0;
        public float StepSize{
            get
            {
                return mStepSize;
            }
        }
        private float mStepSize = 0.0f;
        private List<PropertyNotification> notifications = new List<PropertyNotification>();
        public FishEyeLayoutManager(Size itemSize, View container)
        {
            ItemSize = itemSize;
            Container = container;
            Layout(0.0f);

            mStepSize = Container.Children[0].Size.Height/2.0f + Container.Children[1].Size.Height*Container.Children[1].Scale.X/2.0f;

            foreach(ListItem item in Container.Children)
            {
                PropertyNotification noti = item.AddPropertyNotification("size",PropertyCondition.Step(0.1f));
                noti.Notified += (object source, PropertyNotification.NotifyEventArgs args) =>
                {
                    Layout(mPrevScrollPosition);
                };

                notifications.Add(noti);
            }
        }

        private float FindCandidatePosition(float startPosition, float scrollPosition, bool isBack)
        {
            double center = Math.Abs(scrollPosition);
            double yIntercept = (startPosition - center);
            double result = isBack?center+180.0f:center-180.0f;

            double newFactor = Math.Pow(180,2) / calculateXFactor(0);

            double angle = ItemSize.Height / ItemSize.Width;
            double A = angle;
            double B = yIntercept;
            double D = Math.Sqrt(Math.Pow((180*333),2)/(Math.Pow(333,2)-Math.Pow(153,2)));
            double E = 333.0;
            double F = -153.0;
            double F2 = 153.0;

            double a = Math.Pow(E,2) + Math.Pow(A*D,2);
            double b = -(B*Math.Pow(E,2)+F*Math.Pow(A*D,2));
            double c = Math.Pow(B*E,2) + Math.Pow(A*D*F,2) - Math.Pow(A*D*E,2);
            double b2 = -(B*Math.Pow(E,2)+F2*Math.Pow(A*D,2));
            double c2 = Math.Pow(B*E,2) + Math.Pow(A*D*F2,2) - Math.Pow(A*D*E,2);

            double result1 = (-b+Math.Sqrt((Math.Pow(b,2)-a*c)))/a;
            double result2 = (-b-Math.Sqrt((Math.Pow(b,2)-a*c)))/a;
            double result3 = (-b2+Math.Sqrt((Math.Pow(b2,2)-a*c2)))/a;
            double result4 = (-b2-Math.Sqrt((Math.Pow(b2,2)-a*c2)))/a;

            result = isBack?result1:result4;
            return (float) (center + result);
        }

        protected override void Layout(float scrollPosition)
        {
            // Tizen.Log.Error("NUI","LAYOUT START =================\n");

            ListItem centerItem = Container.Children[FocusedIndex] as ListItem;
            float centerItemPosition = LayoutOrientation == Orientation.Horizontal?centerItem.Position.X:centerItem.Position.Y;

            Vector2 stepRange = new Vector2(-scrollPosition - mStepSize + 1.0f,-scrollPosition + mStepSize - 1.0f);

            if( mStepSize != 0 && centerItemPosition <= stepRange.X)
            {
                Tizen.Log.Error("NUI","[BF]Change CI "+FocusedIndex+"[DI:"+centerItem.DataIndex+"]  : CP "+centerItemPosition+" | range ("+stepRange.X+" ~ "+stepRange.Y+")\n");
                FocusedIndex = Math.Min(Container.Children.Count-1,FocusedIndex+1);
                centerItem = Container.Children[FocusedIndex] as ListItem;

                int newDataIndex = centerItem.DataIndex;
                centerItem.Position = new Position(0.0f,Math.Abs(mStepSize*(newDataIndex)));
                centerItem.Scale = new Vector3(1.0f,1.0f,1.0f);
                Tizen.Log.Error("NUI","[AF]Change CI "+FocusedIndex+"[DI:"+centerItem.DataIndex+"]  : CP "+centerItem.Position.Y+" | range ("+stepRange.X+" ~ "+stepRange.Y+")\n");
            }
            else if(mStepSize != 0 && centerItemPosition >= stepRange.Y)
            {
                Tizen.Log.Error("NUI","[BF]Change CI "+FocusedIndex+"[DI:"+centerItem.DataIndex+"]  : CP "+centerItemPosition+" | range ("+stepRange.X+" ~ "+stepRange.Y+")\n");
                FocusedIndex = Math.Max(0,FocusedIndex-1);
                centerItem = Container.Children[FocusedIndex] as ListItem;

                int newDataIndex = centerItem.DataIndex;
                centerItem.Position = new Position(0.0f,Math.Abs(mStepSize*(newDataIndex)));
                centerItem.Scale = new Vector3(1.0f,1.0f,1.0f);
                Tizen.Log.Error("NUI","[AF]Change CI "+FocusedIndex+"[DI:"+centerItem.DataIndex+"]  : CP "+centerItem.Position.Y+" | range ("+stepRange.X+" ~ "+stepRange.Y+")\n");
            }
            else
            {
                Tizen.Log.Error("NUI","["+scrollPosition+"]No Change CI "+FocusedIndex+"[DI:"+centerItem.DataIndex+"]  : CP "+centerItemPosition+" | range ("+stepRange.X+" ~ "+stepRange.Y+")\n");
                float centerItemScale = calculateScaleFactor(centerItemPosition, scrollPosition);
                centerItem.Scale = new Vector3(centerItemScale,centerItemScale,1.0f);
            }

            ListItem prevItem = centerItem;
            bool visible = true;

            // Tizen.Log.Error("NUI","FocusedIndex : "+FocusedIndex+" scrollPosition : "+scrollPosition+" ==== \n");

            //Center front
            for(int i = FocusedIndex - 1; i > -1; i--)
            {
                // Tizen.Log.Error("NUI","Upper\n");
                ListItem target = Container.Children[i] as ListItem;

                if(visible)
                {
                    float prevItemPosition = LayoutOrientation == Orientation.Horizontal?prevItem.Position.X:prevItem.Position.Y;
                    float prevItemSize = (LayoutOrientation == Orientation.Horizontal?prevItem.Size.Width:prevItem.Size.Height);
                    float prevItemCurrentSize = (LayoutOrientation == Orientation.Horizontal?prevItem.CurrentSizeFloat.Width:prevItem.CurrentSizeFloat.Height);
                    prevItemSize = prevItemCurrentSize == 0 ? prevItemSize : prevItemCurrentSize;
                    prevItemSize = prevItemSize*prevItem.Scale.X;

                    float startPosition = prevItemPosition - prevItemSize / 2.0f;
                    float candidatePosition = Math.Abs(scrollPosition) - 180;
                    float scaleFactor = 0.0f;

                    if(startPosition > Math.Abs(scrollPosition) - 180)
                    {
                        candidatePosition = FindCandidatePosition(startPosition, scrollPosition, false);
                        float candidateHeight = Math.Abs(candidatePosition - startPosition) * 2.0f;
                        float candidateScaleFactor = candidateHeight / ItemSize.Height;
                        scaleFactor = calculateScaleFactor(candidatePosition, scrollPosition);
                    }

                    scaleFactor = calculateScaleFactor(candidatePosition, scrollPosition);

                    // Tizen.Log.Error("NUI","["+i+"] StartPosition ["+startPosition+"] | Candidate ["+candidatePosition+"] | ScaleFactor ["+scaleFactor+"] \n");

                    target.Scale = new Vector3(scaleFactor,scaleFactor,1.0f);

                    target.Position = LayoutOrientation == Orientation.Horizontal?
                                new Position(candidatePosition,target.Position.Y):
                                new Position(target.Position.X,candidatePosition);

                    prevItem = target;

                    visible = scaleFactor > 0.0f ? true:false;
                }
                else
                {
                    target.Scale = new Vector3(0.0f,0.0f,1.0f);
                }
            }

            prevItem = centerItem;
            visible = true;

            //Center back
            for(int i = FocusedIndex + 1; i < Container.Children.Count; i++)
            {
                // Tizen.Log.Error("NUI","Below\n");
                ListItem target = Container.Children[i] as ListItem;

                if(visible)
                {
                    float prevItemPosition = LayoutOrientation == Orientation.Horizontal?prevItem.Position.X:prevItem.Position.Y;
                    float prevItemSize = (LayoutOrientation == Orientation.Horizontal?prevItem.Size.Width:prevItem.Size.Height);
                    float prevItemCurrentSize = (LayoutOrientation == Orientation.Horizontal?prevItem.CurrentSizeFloat.Width:prevItem.CurrentSizeFloat.Height);
                    prevItemSize = prevItemCurrentSize == 0 ? prevItemSize : prevItemCurrentSize;
                    prevItemSize = prevItemSize*prevItem.Scale.X;

                    float startPosition = prevItemPosition + prevItemSize / 2.0f;
                    float candidatePosition = Math.Abs(scrollPosition) + 180;
                    float scaleFactor = 0.0f;

                    if(startPosition < Math.Abs(scrollPosition) + 180)
                    {
                        candidatePosition = FindCandidatePosition(startPosition, scrollPosition, true);
                        float candidateHeight = Math.Abs(candidatePosition - startPosition) * 2.0f;
                        float candidateScaleFactor = candidateHeight / ItemSize.Height;
                        scaleFactor = calculateScaleFactor(candidatePosition, scrollPosition);
                    }

                    // Tizen.Log.Error("NUI","["+i+"] StartPosition ["+startPosition+"] | Candidate ["+candidatePosition+"] | ScaleFactor ["+scaleFactor+"] \n");

                    target.Scale = new Vector3(scaleFactor,scaleFactor,1.0f);

                    target.Position = LayoutOrientation == Orientation.Horizontal?
                                new Position(candidatePosition,target.Position.Y):
                                new Position(target.Position.X,candidatePosition);

                    prevItem = target;

                    visible = scaleFactor > 0.0f ? true:false;
                }
                else
                {
                    target.Scale = new Vector3(0.0f,0.0f,1.0f);
                }
            }

            // Tizen.Log.Error("NUI","LAYOUT END =================\n");
        }

        private double calculateXFactor(double y)
        {
            double factor1 = Math.Pow(180,2);
            double factor2 = Math.Pow(333,2);
            double factor3 = Math.Pow((y + 153),2);

            return Math.Sqrt(factor1-(factor1/factor2*factor3));
        }

        private float calculateScaleFactor(float position, float scrollPosition)
        {
            float origin = Math.Abs(scrollPosition);
            float diff = position - origin;
            
            diff = Math.Max(diff,-180);
            diff = Math.Min(diff,180);
            diff = Math.Abs(diff);

            float result = (float)(calculateXFactor(diff)/calculateXFactor(0));

            if(result < 0.3f)
            {
                result = 0.0f;
            }

            return result;
        }

        public override List<ListItem> OnScroll(float scrollPosition)
        {
            Layout(scrollPosition);

            List<ListItem> result = Recycle(scrollPosition);

            mPrevScrollPosition = scrollPosition;

            return result;
        }

        private List<ListItem> Recycle(float scrollPosition)
        {

            List<ListItem> result = new List<ListItem>();

            bool isBack = scrollPosition - mPrevScrollPosition < 0;

            int previousFocusIndex = FocusedIndex;

            if( !isBack && FocusedIndex < 6)
            {
                ListItem target = Container.Children[Container.Children.Count -1] as ListItem;

                int previousSiblingOrder = target.SiblingOrder;
                target.SiblingOrder = 0;
                target.DataIndex = target.DataIndex - Container.Children.Count;
                target.Position = new Position(0,Math.Abs(scrollPosition)-360);
                target.Scale = new Vector3(0,0,0);

                result.Add(target);

                FocusedIndex++;
            }
            else if( isBack && FocusedIndex > 8)
            {
                ListItem target = Container.Children[0] as ListItem;

                int previousSiblingOrder = target.SiblingOrder;
                target.SiblingOrder = Container.Children.Count - 1;
                target.DataIndex = target.DataIndex + Container.Children.Count;
                target.Position = new Position(0,Math.Abs(scrollPosition)+360);
                target.Scale = new Vector3(0,0,0);

                result.Add(target);

                FocusedIndex--;
            }

            return result;
        }

        public override float CalculateCandidateScrollPosition(float position)
        {
            int value = (int)(Math.Abs(position) / mStepSize);
            float remain = Math.Abs(position) % mStepSize;

            int newValue = remain > mStepSize / 2.0f?value+1:value;

            CurrentFocusedIndex = newValue;
            Tizen.Log.Error("NUI","SCROLL CANDIDATE  =================  "+(-newValue*mStepSize)+"\n");
            return -newValue * mStepSize;
        }
    }
}