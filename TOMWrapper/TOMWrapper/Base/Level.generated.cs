
// Code generated by a template
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using TabularEditor.PropertyGridUI;
using TabularEditor.UndoFramework;
using System.Drawing.Design;
using TOM = Microsoft.AnalysisServices.Tabular;
namespace TabularEditor.TOMWrapper
{
  
    /// <summary>
	/// Base class declaration for Level
	/// </summary>
	[TypeConverter(typeof(DynamicPropertyConverter))]
	public partial class Level: TabularNamedObject
			, IDescriptionObject
			, IAnnotationObject
			, ITranslatableObject
			, IClonableObject
	{
	    protected internal new TOM.Level MetadataObject { get { return base.MetadataObject as TOM.Level; } internal set { base.MetadataObject = value; } }

		public string GetAnnotation(string name) {
		    return MetadataObject.Annotations.Find(name)?.Value;
		}
		public void SetAnnotation(string name, string value, bool undoable = true) {
			if(MetadataObject.Annotations.Contains(name)) {
				MetadataObject.Annotations[name].Value = value;
			} else {
				MetadataObject.Annotations.Add(new TOM.Annotation{ Name = name, Value = value });
			}
			if (undoable) Handler.UndoManager.Add(new UndoAnnotationAction(this, name, value));
		}
		        /// <summary>
        /// Gets or sets the Ordinal of the Level.
        /// </summary>
		[DisplayName("Ordinal")]
		[Category("Other"),IntelliSense("The Ordinal of this Level.")][NoMultiselect()]
		public int Ordinal {
			get {
			    return MetadataObject.Ordinal;
			}
			set {
				var oldValue = Ordinal;
				if (oldValue == value) return;
				bool undoable = true;
				bool cancel = false;
				OnPropertyChanging("Ordinal", value, ref undoable, ref cancel);
				if (cancel) return;
				MetadataObject.Ordinal = value;
				if(undoable) Handler.UndoManager.Add(new UndoPropertyChangedAction(this, "Ordinal", oldValue, value));
				OnPropertyChanged("Ordinal", oldValue, value);
			}
		}
		private bool ShouldSerializeOrdinal() { return false; }
        /// <summary>
        /// Gets or sets the Description of the Level.
        /// </summary>
		[DisplayName("Description")]
		[Category("Basic"),IntelliSense("The Description of this Level.")][Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public string Description {
			get {
			    return MetadataObject.Description;
			}
			set {
				var oldValue = Description;
				if (oldValue == value) return;
				bool undoable = true;
				bool cancel = false;
				OnPropertyChanging("Description", value, ref undoable, ref cancel);
				if (cancel) return;
				MetadataObject.Description = value;
				if(undoable) Handler.UndoManager.Add(new UndoPropertyChangedAction(this, "Description", oldValue, value));
				OnPropertyChanged("Description", oldValue, value);
			}
		}
		private bool ShouldSerializeDescription() { return false; }
        /// <summary>
        /// Gets or sets the Hierarchy of the Level.
        /// </summary>
		[DisplayName("Hierarchy")]
		[Category("Other"),IntelliSense("The Hierarchy of this Level.")][Browsable(false)]
		public Hierarchy Hierarchy {
			get {
				if (MetadataObject.Hierarchy == null) return null;
			    return Handler.WrapperLookup[MetadataObject.Hierarchy] as Hierarchy;
            }
			
		}
		private bool ShouldSerializeHierarchy() { return false; }
        /// <summary>
        /// Gets or sets the Column of the Level.
        /// </summary>
		[DisplayName("Column")]
		[Category("Other"),IntelliSense("The Column of this Level.")][TypeConverter(typeof(HierarchyColumnConverter)),NoMultiselect()]
		public Column Column {
			get {
				if (MetadataObject.Column == null) return null;
			    return Handler.WrapperLookup[MetadataObject.Column] as Column;
            }
			set {
				var oldValue = Column;
				if (oldValue?.MetadataObject == value?.MetadataObject) return;
				bool undoable = true;
				bool cancel = false;
				OnPropertyChanging("Column", value, ref undoable, ref cancel);
				if (cancel) return;
				MetadataObject.Column = value == null ? null : Hierarchy.Table.Columns[value.MetadataObject.Name].MetadataObject;
				if(undoable) Handler.UndoManager.Add(new UndoPropertyChangedAction(this, "Column", oldValue, value));
				OnPropertyChanged("Column", oldValue, value);
			}
		}
		private bool ShouldSerializeColumn() { return false; }
        /// <summary>
        /// Collection of localized descriptions for this Level.
        /// </summary>
        [Browsable(true),DisplayName("Descriptions"),Category("Translations and Perspectives")]
	    public TranslationIndexer TranslatedDescriptions { private set; get; }
        /// <summary>
        /// Collection of localized names for this Level.
        /// </summary>
        [Browsable(true),DisplayName("Names"),Category("Translations and Perspectives")]
	    public TranslationIndexer TranslatedNames { private set; get; }


		public static Level CreateFromMetadata(TOM.Level metadataObject, bool init = true) {
			var obj = new Level(metadataObject, init);
			if(init) 
			{
				obj.InternalInit();
				obj.Init();
			}
			return obj;
		}


		/// <summary>
		/// Creates a new Level and adds it to the parent Hierarchy.
		/// Also creates the underlying metadataobject and adds it to the TOM tree.
		/// </summary>
		public static Level CreateNew(Hierarchy parent, string name = null)
		{
			var metadataObject = new TOM.Level();
			metadataObject.Name = parent.Levels.GetNewName(string.IsNullOrWhiteSpace(name) ? "New Level" : name);

			var obj = new Level(metadataObject);

			parent.Levels.Add(obj);
			
			obj.Init();

			return obj;
		}


		/// <summary>
		/// Creates an exact copy of this Level object.
		/// </summary>
		public Level Clone(string newName = null, bool includeTranslations = true, Hierarchy newParent = null) {
		    Handler.BeginUpdate("Clone Level");

			// Create a clone of the underlying metadataobject:
			var tom = MetadataObject.Clone() as TOM.Level;


			// Assign a new, unique name:
			tom.Name = Parent.Levels.MetadataObjectCollection.GetNewName(string.IsNullOrEmpty(newName) ? tom.Name + " copy" : newName);
				
			// Create the TOM Wrapper object, representing the metadataobject (but don't init until after we add it to the parent):
			var obj = CreateFromMetadata(tom, false);

			// Add the object to the parent collection:
			if(newParent != null) 
				newParent.Levels.Add(obj);
			else
    			Parent.Levels.Add(obj);

			obj.InternalInit();
			obj.Init();
			// Copy translations, if applicable:
			if(includeTranslations) {
				// TODO: Copy translations of child objects

				obj.TranslatedNames.CopyFrom(TranslatedNames);
				obj.TranslatedDescriptions.CopyFrom(TranslatedDescriptions);
			}

            Handler.EndUpdate();

            return obj;
		}

		TabularNamedObject IClonableObject.Clone(string newName, bool includeTranslations, TabularNamedObject newParent) 
		{
			return Clone(newName, includeTranslations);
		}

	
        internal override void RenewMetadataObject()
        {
            Handler.WrapperLookup.Remove(MetadataObject);
            MetadataObject = MetadataObject.Clone() as TOM.Level;
            Handler.WrapperLookup.Add(MetadataObject, this);
        }

		public Hierarchy Parent { 
			get {
				return Handler.WrapperLookup[MetadataObject.Parent] as Hierarchy;
			}
		}



		/// <summary>
		/// CTOR - only called from static factory methods on the class
		/// </summary>
		protected Level(TOM.Level metadataObject, bool init = true) : base(metadataObject)
		{
			if(init) InternalInit();
		}

		private void InternalInit()
		{
			// Create indexers for translations:
			TranslatedNames = new TranslationIndexer(this, TOM.TranslatedProperty.Caption);
			TranslatedDescriptions = new TranslationIndexer(this, TOM.TranslatedProperty.Description);
		}



		internal override void Undelete(ITabularObjectCollection collection) {
			base.Undelete(collection);
			Reinit();
			ReapplyReferences();
		}

		public override bool Browsable(string propertyName) {
			switch (propertyName) {
				case "Parent":
					return false;
				
				// Hides translation properties in the grid, unless the model actually contains translations:
				case "TranslatedNames":
				case "TranslatedDescriptions":
					return Model.Cultures.Any();
				
				default:
					return base.Browsable(propertyName);
			}
		}

    }


	/// <summary>
	/// Collection class for Level. Provides convenient properties for setting a property on multiple objects at once.
	/// </summary>
	public partial class LevelCollection: TabularObjectCollection<Level, TOM.Level, TOM.Hierarchy>
	{
		public override TabularNamedObject Parent { get { return Hierarchy; } }
		public Hierarchy Hierarchy { get; protected set; }
		public LevelCollection(string collectionName, TOM.LevelCollection metadataObjectCollection, Hierarchy parent) : base(collectionName, metadataObjectCollection)
		{
			Hierarchy = parent;
		}

		internal override void Reinit() {
			for(int i = 0; i < Count; i++) {
				var item = this[i];
				Handler.WrapperLookup.Remove(item.MetadataObject);
				item.MetadataObject = Hierarchy.MetadataObject.Levels[i] as TOM.Level;
				Handler.WrapperLookup.Add(item.MetadataObject, item);
				item.Collection = this;
			}
			MetadataObjectCollection = Hierarchy.MetadataObject.Levels;
			foreach(var item in this) item.Reinit();
		}

		internal override void ReapplyReferences() {
			foreach(var item in this) item.ReapplyReferences();
		}

		/// <summary>
		/// Calling this method will populate the LevelCollection with objects based on the MetadataObjects in the corresponding MetadataObjectCollection.
		/// </summary>
		public override void CreateChildrenFromMetadata()
		{
			// Construct child objects (they are automatically added to the Handler's WrapperLookup dictionary):
			foreach(var obj in MetadataObjectCollection) {
				Level.CreateFromMetadata(obj).Collection = this;
			}
		}

		[Description("Sets the Description property of all objects in the collection at once.")]
		public string Description {
			set {
				if(Handler == null) return;
				Handler.UndoManager.BeginBatch(UndoPropertyChangedAction.GetActionNameFromProperty("Description"));
				this.ToList().ForEach(item => { item.Description = value; });
				Handler.UndoManager.EndBatch();
			}
		}

		public override string ToString() {
			return string.Format("({0} {1})", Count, (Count == 1 ? "Level" : "Levels").ToLower());
		}
	}
}
