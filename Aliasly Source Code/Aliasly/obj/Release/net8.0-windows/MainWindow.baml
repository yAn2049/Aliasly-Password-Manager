   M S B A M L   `   `   `  ���� B  >Aliasly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:AliaslyAliasly      Aliasly.MainWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:Aliasly       � Title$  Aliasly��$	��450��$	��800�� � WindowStartupLocation$ CenterScreen=����#212121.��� .+��� -    ��Name$ mesterkulcs_felulet��$y�Visible=�$��Center=�$��Center=�.+��� $��1�� �� Orientation$ 
Horizontal=�$��Center=�$��Center=�.+��� $.��(/images/icons/Aliasly_Logo_Icon_NoBg.png��$��24���� $��Aliasly��$��3q�$��24(�$��Cascadia Mono)�$
��Bold"����#F2F9FF�� $��Enter your master key��$��Center=�$��Center=�$��5q�$��14(�$��Cascadia Mono)�$
��Bold"����#F2F9FFG� -    G�Name$ mesterkulcs_mezo��$��Center=�$��Center=�$��25��$	��300��$��5q�$��14(�$��Cascadia Mono)����#212121���#F2F9FF�� -    ��Name$ 
enter_gomb��$��Enter��$��25��$	��150��$��5q�$��14(�$��Cascadia Mono)����#F2F9FF���#623CEA$��0q��� $��Center=�$��Center=�$��5q�$��14(�$��Cascadia Mono)��� -    ��Name$ uj_kulcs_link�����#F2F9FF.�Create a new key�� -   $ uj_kulcs_felulet��$y�	Collapsed=�$��Center=�$��Center=�.+��� $��1��$ 
Horizontal=�$��Center=�$��Center=�.+��� $.��(/images/icons/Aliasly_Logo_Icon_NoBg.png��$��24���� $��Aliasly��$��3q�$��24(�$��Cascadia Mono)�$
��Bold"����#F2F9FF�� $��Enter a new master key��$��Center=�$��Center=�$��5q�$��14(�$��Cascadia Mono)�$
��Bold"����#F2F9FFG� -   $ uj_kulcs_mezo��$��Center=�$��Center=�$��25��$	��300��$��5q�$��14(�$��Cascadia Mono)����#212121���#F2F9FF�� -   $ uj_kulcs��$��Create new key��$��25��$	��150��$��5q�$��14(�$��Cascadia Mono)����#F2F9FF���#623CEA$��0q��� $��Center=�$��Center=�$��5q�$��14(�$��Cascadia Mono)��� -   $ uj_kulcs_vissza�����#F2F9FF.�Back to entry screen]� -	    ]�Name$ kliens_felulet��$y�	Collapsed=�.+�� $
��Left=� � RowDefinitions �� $��1* ��� $��7* �.+��� $ 
Horizontal=�$��Center=�$��Center=�.+��� $.��(/images/icons/Aliasly_Logo_Icon_NoBg.png��$��28���� $��Aliasly��$��3q�$��28(�$��Cascadia Mono)�$
��Bold"����#F2F9FF�� $��1��$	��250��.+��� $��Center=�.+��� $��Password��$��14(�$��Cascadia Mono)�$
��Bold"����#F2F9FFG� -
   $ jelszo_mezo��$��Center=�$	��200��$��25��$��14(��� $��Center=�$��0,5,0,0q�.+��� $
��Name��$��14(�$��Cascadia Mono)�$
��Bold"����#F2F9FF�� -   	 ��Name$	 nev_mezo��$��Center=�$	��200��$��25��$��14(�$��Cascadia Mono)��� $��Center=�$��0,5,0,0q�.+��� $��Email��$��14(�$��Cascadia Mono)�$
��Bold"����#F2F9FF�� -   $	 
eMail_mezo��$��Center=�$	��200��$��25��$��14(�$��Cascadia Mono)��� $��Center=�$��0,5,0,0q�.+��� $	��Url��$��14(�$��Cascadia Mono)�$
��Bold"����#F2F9FF�� -   $	 url_mezo��$��Center=�$	��200��$��25��$��14(�$��Cascadia Mono)��� $��Center=�$��0,5,0,0q�.+��� $��Description��$��14(�$��Cascadia Mono)�$
��Bold"����#F2F9FF�� -   $	 hozzafuzes_mezo��$��Center=�$	��200��$��25��$��14(�$��Cascadia Mono)��� $ 
Horizontal=�$��Center=�$��Center=�$��0,15,0,0q�.+��� -   $ felhasznalo_rogzites_gomb��$
��Save��$��90��$��28��$��5q�$��14(�$��Cascadia Code)�$
��Bold"����#F2F9FF���#623CEA$��0q�${�False���� -   $ kijelentkezes_gomb��$��Log out��$��90��$��28��$��5q�$��14(�$��Cascadia Mono)�$
��Bold"����#F2F9FF���#623CEA$��0q�� .+��� -   
 ��Name$
 felhasznalok_lista��$��5q�$��0q����� *kliens_lista+ �� View �� -   lista_grid_view��.P��� $��Id�� �� Width$ 25�� �� DisplayMemberBinding �� *
JelszoId+�� $��Password��$ 75�� �� *JelszoString+�� $
��Name��$ 75�� �� *Nev+�� $��E-mail��$	 100�� �� *EMail+�� $	��Url��$ 75�� �� *Url+�� $��Description��$	 100�� �� *
Hozzafuzes+�� $��Delete�� �� CellTemplate �� .R��� $ sor_torles_gomb��-   $��X��$��50������ *
JelszoId+