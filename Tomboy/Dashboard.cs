using System;
using System.Collections.Generic;
using System.Text;
using Mono.Unix;
using Gtk;

namespace Tomboy
{
	public partial class Dashboard : ForcedPresentWindow
	{
		NoteManager manager;
		static Dashboard instance;
		public static Dashboard GetInstance (NoteManager manager)
		{
			if (instance == null)
				instance = new Dashboard (manager);
			System.Diagnostics.Debug.Assert (instance.manager == manager, "Multiple NoteManagers not supported");
			return instance;
		}
		
		protected Dashboard (NoteManager manager) : base(Catalog.GetString ("Search All Notes"))
		{
			this.manager = manager;
			Logger.Debug ("Dashboard initilized with NoteManager");
			// Widget Tomboy.Dashboard
			this.UIManager = new global::Gtk.UIManager ();
			global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
			this.TomboyAction = new global::Gtk.Action ("TomboyAction", global::Mono.Unix.Catalog.GetString ("Tomboy"), null, null);
			this.TomboyAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Tomboy");
			w1.Add (this.TomboyAction, null);
			this.quitAction = new global::Gtk.Action ("quitAction", global::Mono.Unix.Catalog.GetString ("_Quit"), null, "gtk-quit");
			this.quitAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Quit");
			w1.Add (this.quitAction, null);
			this.HelpAction = new global::Gtk.Action ("HelpAction", global::Mono.Unix.Catalog.GetString ("Help"), null, null);
			this.HelpAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Help");
			w1.Add (this.HelpAction, null);
			this.helpAction = new global::Gtk.Action ("helpAction", global::Mono.Unix.Catalog.GetString ("_Help"), null, "gtk-help");
			this.helpAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Help");
			w1.Add (this.helpAction, null);
			this.FileAction = new global::Gtk.Action ("FileAction", global::Mono.Unix.Catalog.GetString ("File"), null, null);
			this.FileAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("File");
			w1.Add (this.FileAction, null);
			this.quitAction1 = new global::Gtk.Action ("quitAction1", global::Mono.Unix.Catalog.GetString ("_Quit"), null, "gtk-quit");
			this.quitAction1.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Quit");
			w1.Add (this.quitAction1, null);
			this.NewNoteAction = new global::Gtk.Action ("NewNoteAction", global::Mono.Unix.Catalog.GetString ("New Note"), null, "note-new");
			this.NewNoteAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("New Note");
			w1.Add (this.NewNoteAction, null);
			this.HelpAction1 = new global::Gtk.Action ("HelpAction1", global::Mono.Unix.Catalog.GetString ("Help"), null, null);
			this.HelpAction1.ShortLabel = global::Mono.Unix.Catalog.GetString ("Help");
			w1.Add (this.HelpAction1, null);
			this.helpAction1 = new global::Gtk.Action ("helpAction1", global::Mono.Unix.Catalog.GetString ("_Contents"), null, "gtk-help");
			this.helpAction1.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Contents");
			w1.Add (this.helpAction1, null);
			this.AboutAction = new global::Gtk.Action ("AboutAction", global::Mono.Unix.Catalog.GetString ("About"), null, null);
			this.AboutAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("About");
			w1.Add (this.AboutAction, null);
			this.ToolsAction = new global::Gtk.Action ("ToolsAction", global::Mono.Unix.Catalog.GetString ("Tools"), null, null);
			this.ToolsAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Tools");
			w1.Add (this.ToolsAction, null);
			this.EditAction = new global::Gtk.Action ("EditAction", global::Mono.Unix.Catalog.GetString ("Edit"), null, null);
			this.EditAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Edit");
			w1.Add (this.EditAction, null);
			this.preferencesAction = new global::Gtk.Action ("preferencesAction", global::Mono.Unix.Catalog.GetString ("Preferences"), null, "gtk-preferences");
			this.preferencesAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Preferences");
			w1.Add (this.preferencesAction, null);
			this.convertAction = new global::Gtk.Action ("convertAction", global::Mono.Unix.Catalog.GetString ("Synchronize Notes"), null, "gtk-convert");
			this.convertAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Synchronize Notes");
			w1.Add (this.convertAction, null);
			this.UIManager.InsertActionGroup (w1, 0);
			this.AddAccelGroup (this.UIManager.AccelGroup);
			this.Name = "Tomboy.Dashboard";
			this.Title = global::Mono.Unix.Catalog.GetString ("Dashboard");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Container child Tomboy.Dashboard.Gtk.Container+ContainerChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.calendar1 = new global::Gtk.Calendar ();
			this.calendar1.CanFocus = true;
			this.calendar1.Name = "calendar1";
			this.calendar1.DisplayOptions = ((global::Gtk.CalendarDisplayOptions)(35));
			this.vbox1.Add (this.calendar1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.calendar1]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.GtkScrolledWindow2 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
			this.GtkScrolledWindow2.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
			this.treeview2 = new global::Gtk.TreeView ();
			this.treeview2.CanFocus = true;
			this.treeview2.Name = "treeview2";
			this.GtkScrolledWindow2.Add (this.treeview2);
			this.vbox1.Add (this.GtkScrolledWindow2);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.GtkScrolledWindow2]));
			w4.Position = 1;
			this.hbox1.Add (this.vbox1);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.vbox1]));
			w5.Position = 0;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox3 = new global::Gtk.HBox ();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.UIManager.AddUiFromString ("<ui><menubar name='menubar2'><menu name='FileAction' action='FileAction'><menuitem name='quitAction1' action='quitAction1'/><menuitem name='NewNoteAction' action='NewNoteAction'/></menu><menu name='EditAction' action='EditAction'><menuitem name='preferencesAction' action='preferencesAction'/></menu><menu name='ToolsAction' action='ToolsAction'><menuitem name='convertAction' action='convertAction'/></menu><menu name='HelpAction1' action='HelpAction1'><menuitem name='helpAction1' action='helpAction1'/><menuitem name='AboutAction' action='AboutAction'/></menu></menubar></ui>");
			this.menubar2 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menubar2")));
			this.menubar2.Name = "menubar2";
			this.hbox3.Add (this.menubar2);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.menubar2]));
			w6.Position = 0;
			w6.Expand = false;
			w6.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.comboboxentry_search = global::Gtk.ComboBoxEntry.NewText ();
			this.comboboxentry_search.Name = "comboboxentry_search";
			this.hbox3.Add (this.comboboxentry_search);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.comboboxentry_search]));
			w7.Position = 2;
			w7.Expand = false;
			w7.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.button_clear_search = new global::Gtk.Button ();
			this.button_clear_search.CanFocus = true;
			this.button_clear_search.Name = "button_clear_search";
			this.button_clear_search.UseUnderline = true;
			// Container child button_clear_search.Gtk.Container+ContainerChild
			global::Gtk.Alignment w8 = new global::Gtk.Alignment (0.5f, 0.5f, 0f, 0f);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w9 = new global::Gtk.HBox ();
			w9.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w10 = new global::Gtk.Image ();
			//w10.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-clear", global::Gtk.IconSize.Menu);
			w9.Add (w10);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w12 = new global::Gtk.Label ();
			w9.Add (w12);
			w8.Add (w9);
			this.button_clear_search.Add (w8);
			this.hbox3.Add (this.button_clear_search);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.button_clear_search]));
			w16.Position = 3;
			w16.Expand = false;
			w16.Fill = false;
			this.vbox2.Add (this.hbox3);
			global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox3]));
			w17.Position = 0;
			w17.Expand = false;
			w17.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.treeview1 = new global::Gtk.TreeView ();
			this.treeview1.WidthRequest = 600;
			this.treeview1.CanFocus = true;
			this.treeview1.Name = "treeview1";
			this.GtkScrolledWindow.Add (this.treeview1);
			this.vbox2.Add (this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.GtkScrolledWindow]));
			w19.Position = 1;
			// Container child vbox2.Gtk.Box+BoxChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.textview1 = new global::Gtk.TextView ();
			this.textview1.CanFocus = true;
			this.textview1.Name = "textview1";
			this.GtkScrolledWindow1.Add (this.textview1);
			this.vbox3.Add (this.GtkScrolledWindow1);
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.GtkScrolledWindow1]));
			w21.Position = 0;
			// Container child vbox3.Gtk.Box+BoxChild
			this.statusbar1 = new global::Gtk.Statusbar ();
			this.statusbar1.Name = "statusbar1";
			this.statusbar1.Spacing = 6;
			this.vbox3.Add (this.statusbar1);
			global::Gtk.Box.BoxChild w22 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.statusbar1]));
			w22.Position = 1;
			w22.Expand = false;
			w22.Fill = false;
			this.vbox2.Add (this.vbox3);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.vbox3]));
			w23.Position = 2;
			this.hbox1.Add (this.vbox2);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.vbox2]));
			w24.Position = 1;
			this.Add (this.hbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 1020;
			this.DefaultHeight = 695;
			this.Show ();
			this.button_clear_search.Clicked += new global::System.EventHandler (this.onClicked_clearSearch);
		
		}
		private global::Gtk.UIManager UIManager;
		
		private global::Gtk.Action TomboyAction;

		private global::Gtk.Action quitAction;

		private global::Gtk.Action HelpAction;

		private global::Gtk.Action helpAction;

		private global::Gtk.Action FileAction;

		private global::Gtk.Action quitAction1;

		private global::Gtk.Action NewNoteAction;

		private global::Gtk.Action HelpAction1;

		private global::Gtk.Action helpAction1;

		private global::Gtk.Action AboutAction;

		private global::Gtk.Action ToolsAction;

		private global::Gtk.Action EditAction;

		private global::Gtk.Action preferencesAction;

		private global::Gtk.Action convertAction;

		private global::Gtk.HBox hbox1;

		private global::Gtk.VBox vbox1;

		private global::Gtk.Calendar calendar1;

		private global::Gtk.ScrolledWindow GtkScrolledWindow2;

		private global::Gtk.TreeView treeview2;

		private global::Gtk.VBox vbox2;

		private global::Gtk.HBox hbox3;

		private global::Gtk.MenuBar menubar2;

		private global::Gtk.ComboBoxEntry comboboxentry_search;

		private global::Gtk.Button button_clear_search;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gtk.TreeView treeview1;

		private global::Gtk.VBox vbox3;

		private global::Gtk.ScrolledWindow GtkScrolledWindow1;

		private global::Gtk.TextView textview1;

		private global::Gtk.Statusbar statusbar1;
		
		protected virtual void onClicked_clearSearch (object sender, System.EventArgs e)
		{
		}
		
	}
}

