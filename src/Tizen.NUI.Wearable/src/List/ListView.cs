
using System;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

using System.Collections.Generic;
using System.ComponentModel;

namespace Tizen.NUI.Wearable
{
    /// <summary>
    /// [Draft] This class provides a View that can scroll a single View with a layout. This View can be a nest of Views.
    /// </summary>
    /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API.
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ListView : ScrollableBase
    {
        private ListAdapter mAdapter;
        private View mContainer;
        protected FishEyeLayoutManager mLayoutManager;
        private int mTotalItemCount = 50;

        public ListView()
        {
            Name = "[List]";
            mContainer = new View()
            {
                WidthSpecification = ScrollingDirection == Direction.Vertical? LayoutParamPolicies.MatchParent:LayoutParamPolicies.WrapContent,
                HeightSpecification = ScrollingDirection == Direction.Horizontal? LayoutParamPolicies.MatchParent:LayoutParamPolicies.WrapContent,
                Layout = new AbsoluteLayout()
                {
                    SetPositionByLayout = false,
                },
                Name="Container",
                PositionUsesPivotPoint = true,
                ParentOrigin = Tizen.NUI.ParentOrigin.Center,
                PivotPoint = ScrollingDirection == Direction.Vertical?Tizen.NUI.PivotPoint.TopCenter:Tizen.NUI.PivotPoint.CenterLeft,
            };

            Add(mContainer);
            ScrollEvent += OnScroll;
            ScrollDragStartEvent += OnScrollDragStart;
            ScrollAnimationEndEvent += OnAnimationEnd;
        }

        public ListAdapter Adapter{
            get
            {
                return mAdapter;
            }
            set
            {
                mAdapter = value;
                mAdapter.OnDataChanged += OnAdapterDataChanged;
                InitializeChild();
            }
        }

        public Size ItemSize{get;set;} = new Size();


        private void InitializeChild()
        {
            ItemSize = mAdapter.CreateListItem().Size;

            if(ScrollingDirection == Direction.Horizontal)
            {
                mContainer.WidthSpecification = (int)(ItemSize.Width * mAdapter.Data.Count);
            }
            else
            {
                mContainer.HeightSpecification = (int)(ItemSize.Height * mAdapter.Data.Count);
            }

            for(int i = 0; i< mTotalItemCount; i++)
            {
                ListItem item = mAdapter.CreateListItem();
                item.DataIndex = i;
                item.Name ="["+i+"] recycle";

                if(i<mAdapter.Data.Count)
                {
                    mAdapter.BindData(item);
                }
                mContainer.Add(item);
            }

            mLayoutManager = new FishEyeLayoutManager(ItemSize,mContainer);
            mContainer.Size = new Size(mContainer.Size.Width,mLayoutManager.StepSize*(mAdapter.Data.Count-1));

            ListItem focusedItem = mContainer.Children[0] as ListItem;
            focusedItem.OnFocusGained();
        }

        private void OnScroll(object source, ScrollableBase.ScrollEventArgs args)
        {
            List<ListItem> recycledItemList = mLayoutManager.OnScroll(ScrollingDirection == Direction.Horizontal?args.Position.X:args.Position.Y);
            BindData(recycledItemList);
        }

        private void OnAdapterDataChanged(object source, EventArgs args)
        {
            List<ListItem> changedData = new List<ListItem>();

            foreach(ListItem item in mContainer.Children)
            {
                changedData.Add(item);
            }

            BindData(changedData);
        }

        private void BindData(List<ListItem> changedData)
        {
            foreach(ListItem item in changedData)
            {
                if(item.DataIndex > -1 && item.DataIndex < mAdapter.Data.Count)
                {
                    item.Show();
                    mAdapter.BindData(item);
                }
                else
                {
                    item.Hide();
                }
            }
        }

        protected override float AdjustScrollToChild(float position)
        {
            return mLayoutManager.CalculateCandidateScrollPosition(position);
        }


        private void OnAnimationEnd(object source, ScrollableBase.ScrollEventArgs args)
        {
            Tizen.Log.Error("NUI","ANIMATION ENDED  =================  "+(args.Position.Y)+"\n");

            FishEyeLayoutManager layoutManager = mLayoutManager as FishEyeLayoutManager;

            if(layoutManager != null)
            {
                ListItem focusedItem = mContainer.Children[layoutManager.FocusedIndex] as ListItem;
                focusedItem.OnFocusGained();
            }
        }

        private void OnScrollDragStart(object source, ScrollableBase.ScrollEventArgs args)
        {
            FishEyeLayoutManager layoutManager = mLayoutManager as FishEyeLayoutManager;

            if(layoutManager != null)
            {
                ListItem focusedItem = mContainer.Children[layoutManager.FocusedIndex] as ListItem;
                focusedItem.OnFocusLost();
            }
        }
    }
}