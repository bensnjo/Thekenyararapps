﻿using Xamarin.Forms;

namespace EBM2x.UI.Draw
{
    public class BackofficeLine60x1Grid : Grid
    {
        int rowCount = 1;
        int colCount = 60;

        public BackofficeLine60x1Grid()
        {
            this.RowSpacing = 0;
            this.ColumnSpacing = 0;

            for (int i = 0; i < rowCount; i++)
            {
                RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < colCount; i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition());
            }
        }
    }
}
