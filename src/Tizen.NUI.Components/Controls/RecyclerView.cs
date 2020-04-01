
using System;
using Tizen.NUI.BaseComponents;

using System.Collections.Generic;
using System.ComponentModel;

namespace Tizen.NUI.Components
{
    /// <summary>
    /// [Draft] This class provides a View that can scroll a single View with a layout. This View can be a nest of Views.
    /// </summary>
    /// This may be public opened in tizen_6.0 after ACR done. Before ACR, need to be hidden as inhouse API.
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class RecyclerView : ScrollableBase
    {
        private Adapter mAdapter;
        private View mContainer;
        private Size mListItemSize;

        public class Item : Control
        {
            public Item()
            {
                
            }

            public int DataIndex{get;set;}=0;

            protected override bool OnControlStateChanged(ControlStates currentState)
            {
                return false;
            }
        }

        public class Adapter
        {
            private List<object> mData = new List<object>();
            public Adapter()
            {
            }

            public virtual Item CreateItem()
            {
                return new Item();
            }

            public virtual void BindData(Item item, int index)
            {

            }

            public void Notify()
            {
                OnDataChanged?.Invoke(this, new EventArgs());
            }

            public event EventHandler<EventArgs> OnDataChanged;

            public List<object> Data{
                get
                {
                    return mData;
                }
                set
                {
                    mData = value;
                    Notify();
                }
            }
            
        }

        public RecyclerView()
        {
            Name = "[RecyclerView]";
            mContainer = new View()
            {
                WidthSpecification = ScrollingDirection == Direction.Vertical? LayoutParamPolicies.MatchParent:LayoutParamPolicies.WrapContent,
                HeightSpecification = ScrollingDirection == Direction.Horizontal? LayoutParamPolicies.MatchParent:LayoutParamPolicies.WrapContent,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                },
                Name="Container"
            };
            ScrollEvent += OnScroll;
        }

        public Adapter RecyclerAdapter{
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

        public new LayoutItem Layout{
            get
            {
                LayoutItem result = base.Layout;
                if(mContainer)
                {
                    result = mContainer.Layout;
                }

                return result;
            }
            set
            {
                if(mContainer)
                {
                    mContainer.Layout = value;
                }
            }
        }

        public int SpareItemCount { get; set; } = 5;
        private int mTotalItemCount = 0;

        private void InitializeChild()
        {
            mListItemSize = mAdapter.CreateItem().Size;
            if(ScrollingDirection == Direction.Horizontal)
            {
                mContainer.WidthSpecification = (int)(mListItemSize.Width * mAdapter.Data.Count);
            }
            else
            {
                mContainer.HeightSpecification = (int)(mListItemSize.Height * mAdapter.Data.Count);
            }
            mTotalItemCount = CalculateTotalItemCount();
            Add(mContainer);

            for(int i = 0; i< mTotalItemCount && i < mAdapter.Data.Count; i++)
            {
                Item item = mAdapter.CreateItem();
                item.DataIndex = i;
                item.Name ="["+i+"] recycle";
                mAdapter.BindData(item, i);
                mContainer.Add(item);
            }
        }

        private int CalculateTotalItemCount()
        {
            int visibleItemCount = 0;

            if(ScrollingDirection == Direction.Horizontal)
            {
                visibleItemCount = (int)(Size.Width/mListItemSize.Width);
            }
            else
            {
                visibleItemCount = (int)(Size.Height/mListItemSize.Height);
            }
            
            return visibleItemCount + SpareItemCount * 2;
        }

        private void OnScroll(object source, ScrollableBase.ScrollEventArgs args)
        {
            LayoutGroup containerLayout = mContainer.Layout as LayoutGroup;

            List<KeyValuePair<int,View>> changedData = containerLayout.RecycleItemByCurrentPosition(args.Position, SpareItemCount);

            if(changedData.Count > 0)
            {
                BindData(changedData);
            }
        }

        private void OnAdapterDataChanged(object source, EventArgs args)
        {
            List<KeyValuePair<int,View>> changedData = new List<KeyValuePair<int,View>>();

            foreach(Item item in mContainer.Children)
            {
                changedData.Add( new KeyValuePair<int,View>( item.DataIndex,item ) );
            }

            BindData(changedData);
        }

        private void BindData(List<KeyValuePair<int,View>> changedData)
        {
            foreach(KeyValuePair<int,View> data in changedData)
            {
                if(data.Key > -1 && data.Key < mAdapter.Data.Count)
                {
                    Item item = data.Value as Item;
                    item.DataIndex = data.Key;

                    mAdapter.BindData(data.Value as Item, data.Key);
                }
            }
        }
    }
}