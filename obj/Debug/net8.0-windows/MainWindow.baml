   M S B A M L   `   `   `  ����F  BPaintBetter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:PaintBetterPaintBetter  5   	       PaintBetter.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:PaintBetter     5   	   -   5          � Title$  PaintBetter��5   	   $	��600��6   $	��800��6*    � WindowState$ 	Maximized=�5	   	      ThemeWindowBackgroundBrush#���   5
   	     ThemeForegroundBrush#���  5   	   .��5      � -   5        .+�5   
   ]� -   5         ]�Name$ MainContentPanel��5      .+�5      `� -   5         `�Name$ MainToolBarTray��5      $	��Top=�63     ThemeControlBackgroundBrush `� 
Background# �  6H   .�5      c� $��60��6   $��5q�6&   .@�5      �� -   5         ��Name$ 
FileButton��5       � ToolTip$ 	File Menu��6J   $��18(�6^   $��35��6l   $��35��6w   $��Center=�6�   $��Center=�6�   .��5      �� $	��☰��6%   $��Segoe UI Symbol)�6.   5      �� 5      �� 5      -   5        $ PencilButton��5      $ Pencil Tool��63   $��18(�6I   $��35��6W   $��35��6b   $��Center=�6n   $��Center=�6�   .��5      �� $��✏️��6$   $��Segoe UI Symbol)�6.   5      �� 5      -   5        $ EraserButton��5      $ Eraser Tool��63   $��18(�6I   $��35��6W   $��35��6b   $��Center=�6n   $��Center=�6�   .��5      �� $	��⬜��6$   $��Segoe UI Symbol)�6-   $ Eraser Placeholder��6J   5      �� 5      �� 5       �� Orientation$ Vertical=�6!   $	��5,0q�68   .+�5      �� $��Color��6%   $��9(�62   $��Center=�6?   �� 5       -   5         ��Name$ PrimaryColorIndicator��5    "   $��25��6A   $��15��6L   
���   �
�������$��1q�6~   $��0,1,0,1q�6�   $ Primary Color��6�   �� 5!      -	   5        $ SecondaryColorIndicator��5!   "   $��25��6C   $��15��6N   
�������
�������$��1q�6�   $' !Secondary Color (Not Implemented)��6�   5"      �� 5#      � 5$      -
   5        	 �Name$	 ColorPalettePanel��5$   !   
 � Orientation$
 
Horizontal=�6<   $��Center=�6U   $	��150��6p   .+�5%      �� -   5        $ ColorSwatchBlack��5%   "   $��20��6<   $��20��6G   
���   �
�������$��1q�6y   $��1q�6�    � Cursor$
 Hand��6�   6�   �� 5&      -   5        $ ColorSwatchWhite��5&   "   $��20��6<   $��20��6G   
�������
�������$��1q�6y   $��1q�6�   $
 Hand��6�   6�   �� 5'      -   5        $ ColorSwatchRed��5'   "   $��20��6:   $��20��6E   
���  ��
�������$��1q�6u   $��1q�6�   $
 Hand��6�   6�   �� 5(      -   5        $ ColorSwatchGreen��5(   "   $��20��6<   $��20��6G   
��� � �
�������$��1q�6x   $��1q�6�   $
 Hand��6�   6�   �� 5)      -   5        $ ColorSwatchBlue��5)   "   $��20��6;   $��20��6F   
����  �
�������$��1q�6w   $��1q�6�   $
 Hand��6�   6�   �� 5*      -   5        $ ColorSwatchYellow��5*   "   $��20��6=   $��20��6H   
��� ���
�������$��1q�6{   $��1q�6�   $
 Hand��6�   6�   5+      �� 5,      �� 5-      $��Width:��6   $��Center=�6-   �� 5.      -   5         ��Name$ WidthTextBox��5.      $��40��64   $��Center=�6?   �� 5/      $��Height:��6   $��Center=�6.   �� 50      -   5        $ HeightTextBox��50      $��40��65   $��Center=�6@   �� 51      -   5        $ ResizeButton��51      $ Set Canvas Size��6N   $��18(�6h   $��35��6v   $��35��6�   $��Center=�6�   $��Center=�6�   .��52      �� $��↔️��6%   $��Segoe UI Symbol)�6/   53      54      55      �� 57      -   5         ��Name$ MainStatusBar��57      $��Bottom=�6/   #���  6G   .@�58      �� .��59      �� -   5         ��Name$ FileNameText��59       $��Untitled��66   5:      �� 5;      �� 5<      .��5=      �� -   5        $ CoordinatesText��5=       $��
X: 0, Y: 0��69   5>      5?      �� 5B      -   5         ��Name$ CanvasScrollViewer��5B      $
��Auto=�5C      $
��Auto=�5D      #���   5E      .��5L      �� -   5         ��Name$ CanvasContainerPanel��5L      
������ ��5M      �� *ActualWidth+ �� ElementName$ CanvasScrollViewer�� �� Mode$ OneWay=���5N      �� *ActualHeight+$ CanvasScrollViewer��$ OneWay=�.+�5Q      �� -   5        $ CanvasBorder��5Q      $	��800��5R      $	��600��6)     ThemeBorderBrush#���  5S      $��1q�6N    �� Left$ 0��5T      
 �� Top$ 0��6-    � LayoutTransform 5U      �� 5V      -   5        CanvasScaleTransform�� �� ScaleX$ 1Q�5V   K    �� ScaleY$ 1Q�6V   ._�5Y      �� -   5         ��Name$ DrawingCanvas��5Y   $    �� 
Background
 �����6N   5Z      5[      5\      5]      �� 5`   
   -   5        $ 	FilePanel��5`      $	��250��5a      $
��Left=�5b      #���  5c      #���  5d      $��0,0,1,0q�5e      z�5f      T� 5g      -   5        FilePanelTranslateTransform�� T� X$
 -250Q�5g   J   ._�5i      ]� $��10q�6   .+�5j      �� -   5        $ CloseFilePanelButton��5j      $��Bottom=�68   $��Close��6P   $��0,10,0,0q�6`   $��Stretch=�6�   �� 5k      -   5        $ FilePanelQuitButton��5k      $��Bottom=�67   $
��Quit��6O   $��0,5,0,0q�6^   $��Stretch=�6�   �� 5n      $��Bottom=�6   $��0,20,0,0q�66   .+�5o      �� $��Settings��6   $��SemiBold"�60   $��0,0,0,5q�6F   	� 5p      -    5         	�Name$ LightThemeRadio��5p   #   $��Light Theme��6<    	� 	GroupName$ Theme��6R    d� 	IsChecked$
 TrueY�6�   	� 5q      -!   5        $ DarkThemeRadio��5q   #   $��
Dark Theme��6;   $ Theme��6P   6b   5r      �� 5u      -"   5        $ FilePanelSaveButton��5u      $
��Save��67   $��50��6F   $��16(�6R   $��0,0,0,10q�6`   $	��Top=�6r   $��Stretch=�6�   5w      5x      5y      5z      