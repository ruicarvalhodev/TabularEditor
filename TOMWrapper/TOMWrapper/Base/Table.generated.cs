
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
	/// Base class declaration for Table
	/// </summary>
	[TypeConverter(typeof(DynamicPropertyConverter))]
	public partial class Table: TabularNamedObject
			, IHideableObject
			, IDescriptionObject
			, IAnnotationObject
			, ITabularPerspectiveObject
			, ITranslatableObject
			, IClonableObject
	{
	    protected internal new TOM.Table MetadataObject { get { return base.MetadataObject as TOM.Table; } internal set { base.MetadataObject = value; } }

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
        /// Gets or sets the DataCategory of the Table.
        /// </summary>
		[DisplayName("Data Category")]
		[Category("Metadata"),IntelliSense("The Data Category of this Table.")]
		public string DataCategory {
			get {
			    return MetadataObject.DataCategory;
			}
			set {
				var oldValue = DataCategory;
				if (oldValue == value) return;
				bool undoable = true;
				bool cancel = false;
				OnPropertyChanging("DataCategory", value, ref undoable, ref cancel);
				if (cancel) return;
				MetadataObject.DataCategory = value;
				if(undoable) Handler.UndoManager.Add(new UndoPropertyChangedAction(this, "DataCategory", oldValue, value));
				OnPropertyChanged("DataCategory", oldValue, value);
			}
		}
		private bool ShouldSerializeDataCategory() { return false; }
        /// <summary>
        /// Gets or sets the Description of the Table.
        /// </summary>
		[DisplayName("Description")]
		[Category("Basic"),IntelliSense("The Description of this Table.")][Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
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
        /// Gets or sets the IsHidden of the Table.
        /// </summary>
		[DisplayName("Hidden")]
		[Category("Basic"),IntelliSense("The Hidden of this Table.")]
		public bool IsHidden {
			get {
			    return MetadataObject.IsHidden;
			}
			set {
				var oldValue = IsHidden;
				if (oldValue == value) return;
				bool undoable = true;
				bool cancel = false;
				OnPropertyChanging("IsHidden", value, ref undoable, ref cancel);
				if (cancel) return;
				MetadataObject.IsHidden = value;
				if(undoable) Handler.UndoManager.Add(new UndoPropertyChangedAction(this, "IsHidden", oldValue, value));
				OnPropertyChanged("IsHidden", oldValue, value);
				Handler.UpdateObject(this);
			}
		}
		private bool ShouldSerializeIsHidden() { return false; }
        /// <Summary>
		/// Collection of perspectives in which this Table is visible.
		/// </Summary>
		[Browsable(true),DisplayName("Perspectives"), Category("Translations and Perspectives")]
        public PerspectiveIndexer InPerspective { get; private set; }
        /// <summary>
        /// Collection of localized descriptions for this Table.
        /// </summary>
        [Browsable(true),DisplayName("Descriptions"),Category("Translations and Perspectives")]
	    public TranslationIndexer TranslatedDescriptions { private set; get; }
        /// <summary>
        /// Collection of localized names for this Table.
        /// </summary>
        [Browsable(true),DisplayName("Names"),Category("Translations and Perspectives")]
	    public TranslationIndexer TranslatedNames { private set; get; }


		public static Table CreateFromMetadata(TOM.Table metadataObject, bool init = true) {
			var obj = new Table(metadataObject, init);
			if(init) 
			{
				obj.InternalInit();
				obj.Init();
			}
			return obj;
		}


		/// <summary>
		/// Creates a new Table and adds it to the parent Model.
		/// Also creates the underlying metadataobject and adds it to the TOM tree.
		/// </summary>
		public static Table CreateNew(Model parent, string name = null)
		{
			var metadataObject = new TOM.Table();
			metadataObject.Name = parent.Tables.GetNewName(string.IsNullOrWhiteSpace(name) ? "New Table" : name);

			var obj = new Table(metadataObject);

			parent.Tables.Add(obj);
			
			obj.Init();

			return obj;
		}

		/// <summary>
		/// Creates a new Table and adds it to the current Model.
		/// Also creates the underlying metadataobject and adds it to the TOM tree.
		/// </summary>		
		public static Table CreateNew(string name = null)
		{
			var metadataObject = new TOM.Table();
			metadataObject.Name = TabularModelHandler.Singleton.Model.Tables.GetNewName(string.IsNullOrWhiteSpace(name) ? "New Table" : name);

			var obj = new Table(metadataObject);

			TabularModelHandler.Singleton.Model.Tables.Add(obj);

			obj.Init();

			return obj;
		}


		/// <summary>
		/// Creates an exact copy of this Table object.
		/// </summary>
		public Table Clone(string newName = null, bool includeTranslations = true) {
		    Handler.BeginUpdate("Clone Table");

			// Create a clone of the underlying metadataobject:
			var tom = MetadataObject.Clone() as TOM.Table;

			// Make sure that measures on the table are renamed:
			foreach(var m in tom.Measures) m.Name = MeasureCollection.GetNewMeasureName(m.Name);

			// Assign a new, unique name:
			tom.Name = Parent.Tables.MetadataObjectCollection.GetNewName(string.IsNullOrEmpty(newName) ? tom.Name + " copy" : newName);
				
			// Create the TOM Wrapper object, representing the metadataobject (but don't init until after we add it to the parent):
			var obj = CreateFromMetadata(tom, false);

			// Add the object to the parent collection:
			Parent.Tables.Add(obj);

			obj.InternalInit();
			obj.Init();
			// Copy translations, if applicable:
			if(includeTranslations) {
				// TODO: Copy translations of child objects

				obj.TranslatedNames.CopyFrom(TranslatedNames);
				obj.TranslatedDescriptions.CopyFrom(TranslatedDescriptions);
			}
				
			// Copy perspectives:
			obj.InPerspective.CopyFrom(InPerspective);

            Handler.EndUpdate();

            return obj;
		}

		TabularNamedObject IClonableObject.Clone(string newName, bool includeTranslations, TabularNamedObject newParent) 
		{
			if (newParent != null) throw new ArgumentException("This object can not be cloned to another parent. Argument newParent should be left as null.", "newParent");
			return Clone(newName, includeTranslations);
		}

	
        internal override void RenewMetadataObject()
        {
            Handler.WrapperLookup.Remove(MetadataObject);
            MetadataObject = MetadataObject.Clone() as TOM.Table;
            Handler.WrapperLookup.Add(MetadataObject, this);
        }

		public Model Parent { 
			get {
				return Handler.WrapperLookup[MetadataObject.Parent] as Model;
			}
		}

        internal override ITabularObjectCollection GetCollectionForChild(TabularObject child)
        {
			if (child is Partition) return Partitions;
			if (child is Column) return Columns;
			if (child is Hierarchy) return Hierarchies;
			if (child is Measure) return Measures;
            return base.GetCollectionForChild(child);
        }

        /// <summary>
        /// The collection of Partition objects on this Table.
        /// </summary>
		[DisplayName("Partitions")]
		[Category("Data Source"),IntelliSense("The collection of Partition objects on this Table.")][NoMultiselect(),Editor(typeof(PartitionCollectionEditor),typeof(UITypeEditor))]
		public PartitionCollection Partitions { get; protected set; }
        /// <summary>
        /// The collection of Column objects on this Table.
        /// </summary>
		[DisplayName("Columns")]
		[Category("Other"),IntelliSense("The collection of Column objects on this Table.")]
		public ColumnCollection Columns { get; protected set; }
        /// <summary>
        /// The collection of Hierarchy objects on this Table.
        /// </summary>
		[DisplayName("Hierarchies")]
		[Category("Other"),IntelliSense("The collection of Hierarchy objects on this Table.")]
		public HierarchyCollection Hierarchies { get; protected set; }
        /// <summary>
        /// The collection of Measure objects on this Table.
        /// </summary>
		[DisplayName("Measures")]
		[Category("Other"),IntelliSense("The collection of Measure objects on this Table.")]
		public MeasureCollection Measures { get; protected set; }

		/// <summary>
		/// CTOR - only called from static factory methods on the class
		/// </summary>
		protected Table(TOM.Table metadataObject, bool init = true) : base(metadataObject)
		{
			if(init) InternalInit();
		}

		private void InternalInit()
		{
			// Create indexers for translations:
			TranslatedNames = new TranslationIndexer(this, TOM.TranslatedProperty.Caption);
			TranslatedDescriptions = new TranslationIndexer(this, TOM.TranslatedProperty.Description);

			// Create indexers for perspectives:
			InPerspective = new PerspectiveTableIndexer(this);
			
			// Instantiate child collections:
			Partitions = new PartitionCollection(this.GetObjectPath() + ".Partitions", MetadataObject.Partitions, this);
			Columns = new ColumnCollection(this.GetObjectPath() + ".Columns", MetadataObject.Columns, this);
			Hierarchies = new HierarchyCollection(this.GetObjectPath() + ".Hierarchies", MetadataObject.Hierarchies, this);
			Measures = new MeasureCollection(this.GetObjectPath() + ".Measures", MetadataObject.Measures, this);

			// Populate child collections:
			Partitions.CreateChildrenFromMetadata();
			Columns.CreateChildrenFromMetadata();
			Hierarchies.CreateChildrenFromMetadata();
			Measures.CreateChildrenFromMetadata();

			// Hook up event handlers on child collections:
			Partitions.CollectionChanged += Children_CollectionChanged;
			Columns.CollectionChanged += Children_CollectionChanged;
			Hierarchies.CollectionChanged += Children_CollectionChanged;
			Measures.CollectionChanged += Children_CollectionChanged;
		}


		internal override void Reinit() {
			Partitions.Reinit();
			Columns.Reinit();
			Hierarchies.Reinit();
			Measures.Reinit();
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
				
				// Hides the perspective property in the grid, unless the model actually contains perspectives:
				case "InPerspective":
					return Model.Perspectives.Any();
				
				default:
					return base.Browsable(propertyName);
			}
		}

    }


	/// <summary>
	/// Collection class for Table. Provides convenient properties for setting a property on multiple objects at once.
	/// </summary>
	public partial class TableCollection: TabularObjectCollection<Table, TOM.Table, TOM.Model>
	{
		public override TabularNamedObject Parent { get { return Model; } }
		public TableCollection(string collectionName, TOM.TableCollection metadataObjectCollection, Model parent) : base(collectionName, metadataObjectCollection)
		{
		}

		internal override void Reinit() {
			for(int i = 0; i < Count; i++) {
				var item = this[i];
				Handler.WrapperLookup.Remove(item.MetadataObject);
				item.MetadataObject = Model.MetadataObject.Tables[i] as TOM.Table;
				Handler.WrapperLookup.Add(item.MetadataObject, item);
				item.Collection = this;
			}
			MetadataObjectCollection = Model.MetadataObject.Tables;
			foreach(var item in this) item.Reinit();
		}

		internal override void ReapplyReferences() {
			foreach(var item in this) item.ReapplyReferences();
		}

		/// <summary>
		/// Calling this method will populate the TableCollection with objects based on the MetadataObjects in the corresponding MetadataObjectCollection.
		/// </summary>
		public override void CreateChildrenFromMetadata()
		{
			// Construct child objects (they are automatically added to the Handler's WrapperLookup dictionary):
			foreach(var obj in MetadataObjectCollection) {
				switch(obj.GetSourceType()) {
				    case TOM.PartitionSourceType.Calculated: CalculatedTable.CreateFromMetadata(obj).Collection = this; break;
					default: Table.CreateFromMetadata(obj).Collection = this; break;
				}
			}
		}

		[Description("Sets the DataCategory property of all objects in the collection at once.")]
		public string DataCategory {
			set {
				if(Handler == null) return;
				Handler.UndoManager.BeginBatch(UndoPropertyChangedAction.GetActionNameFromProperty("DataCategory"));
				this.ToList().ForEach(item => { item.DataCategory = value; });
				Handler.UndoManager.EndBatch();
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
		[Description("Sets the IsHidden property of all objects in the collection at once.")]
		public bool IsHidden {
			set {
				if(Handler == null) return;
				Handler.UndoManager.BeginBatch(UndoPropertyChangedAction.GetActionNameFromProperty("IsHidden"));
				this.ToList().ForEach(item => { item.IsHidden = value; });
				Handler.UndoManager.EndBatch();
			}
		}

		public override string ToString() {
			return string.Format("({0} {1})", Count, (Count == 1 ? "Table" : "Tables").ToLower());
		}
	}
}
