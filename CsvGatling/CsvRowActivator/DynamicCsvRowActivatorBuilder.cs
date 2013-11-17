using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using System.IO;
using System.ComponentModel;

namespace CsvGatling
{
    public class DynamicCsvRowActivatorBuilder<T>
    {
        private readonly IConverterProvider converterProvider;
        private readonly IColumnNameMapper columnNameMapper;

        public DynamicCsvRowActivatorBuilder(IConverterProvider converterProvider, IColumnNameMapper columnNameMapper)
        {
            if (converterProvider == null)
            {
                throw new ArgumentNullException("converterProvider");
            }
            if (columnNameMapper == null)
            {
                throw new ArgumentNullException("columnNameMapper");
            }
            this.converterProvider = converterProvider;
            this.columnNameMapper = columnNameMapper;
        }

        public TypeBuilder Build(AssemblyBuilder assemblyBuilder, ModuleBuilder moduleBuilder)
        {
            PropertyInfo[] propertiesOfT = typeof(T).GetProperties().Where(p => p.CanWrite).ToArray();
            TypeBuilder classBuilder = BuildClass(moduleBuilder);
            FieldBuilder[] indexFields = BuildIndexFields(propertiesOfT, classBuilder);
            FieldBuilder[] columnNameFields = BuildColumnNameFields(propertiesOfT, classBuilder);
            ConstructorBuilder constructorBuilder = BuildConstructor(classBuilder, indexFields, columnNameFields, propertiesOfT);
            MethodBuilder createFromRowMethodBuilder = BuildMethodCreateFromRow(classBuilder, indexFields, columnNameFields, propertiesOfT);
            return classBuilder;
        }

        private TypeBuilder BuildClass(ModuleBuilder module)
        {
            TypeBuilder type = module.DefineType(
                "TryDynamicCsvGatling.DynamicCsvReaderActivator", 
                TypeAttributes.Public
                | TypeAttributes.Class
                | TypeAttributes.AutoClass
                | TypeAttributes.AnsiClass
                | TypeAttributes.BeforeFieldInit
                | TypeAttributes.AutoLayout, 
                typeof(Object),   
                new Type[]{
                    typeof(ICsvRowActivator<T>)
                    }
                );
            return type;
        }
        public FieldBuilder[] BuildIndexFields(PropertyInfo[] propertiesOfT, TypeBuilder type)
        {
            return propertiesOfT.Select((pi, i) =>
            {
                return type.DefineField("index" +i, typeof(int), FieldAttributes.Private | FieldAttributes.InitOnly);
            }).ToArray();
        }
        public FieldBuilder[] BuildColumnNameFields(PropertyInfo[] propertiesOfT, TypeBuilder type)
        {
            return propertiesOfT.Select((pi, i) =>
            {
                return type.DefineField("columnName" + i, typeof(string), FieldAttributes.Private | FieldAttributes.InitOnly);
            }).ToArray();
        }


        public ConstructorBuilder BuildConstructor(
            TypeBuilder type, 
            FieldBuilder[] indexFields, 
            FieldBuilder[] columnNameFields, 
            PropertyInfo[] propertiesOfT)
        {
            // Create Constructor
            ConstructorBuilder constructor = type.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig, 
                CallingConventions.HasThis, 
                new Type[] { typeof(string[]) });
            
            // Parameter csvHeader
            ILGenerator gen = constructor.GetILGenerator();

            // Call base constructor
            ConstructorInfo systemObjectConstructor = typeof(object).GetConstructor(Type.EmptyTypes);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Call, systemObjectConstructor);

            MethodInfo stringEqualityOperator = typeof(String).GetMethod(
                "op_Equality",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{
                    typeof(String),
                    typeof(String)
                    },
                null
            );

            //this.indexClientId = -1;
            //this.indexBirthDate = -1;
            //this.indexWeight = -1;
            //this.indexReportName = -1;
            foreach(FieldBuilder field in indexFields)
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldc_I4_M1);
                gen.Emit(OpCodes.Stfld,field);
            }

            //this.csvColumnNameClientId = "ClientId";
            //this.csvColumnNameBirthDate = "BirthDate";
            //this.csvColumnNameWeight = "Weight";
            //this.csvColumnNameReportName = "ReportName";
            for (int i = 0; i < indexFields.Length; i++)
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldstr, columnNameMapper.GetColumnNameFor(propertiesOfT[i]));
                gen.Emit(OpCodes.Stfld, columnNameFields[i]);
            }
            
            LocalBuilder forLoopI = gen.DeclareLocal(typeof(Int32));

            // Preparing labels
            Label maybeRepeatForLoop = gen.DefineLabel();
            Label incrementForLoop = gen.DefineLabel();
            Label repeatForLoop = gen.DefineLabel();

            //for (int i = 0; i < csvHeader.Length; i++)
            //{
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Stloc_0);
            gen.Emit(OpCodes.Br, maybeRepeatForLoop);
            gen.MarkLabel(repeatForLoop);
            //    if (indexClientId < 0 && csvHeader[i] == this.csvColumnNameClientId)
            //    {
            //        indexClientId = i;
            //    }
            for (int i = 0; i < indexFields.Length; i++)
            {
                Label endOfIf = gen.DefineLabel();
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, indexFields[i]); // index
                gen.Emit(OpCodes.Ldc_I4_0);
                gen.Emit(OpCodes.Bge_S, endOfIf);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldloc, forLoopI);
                gen.Emit(OpCodes.Ldelem_Ref);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, columnNameFields[i]); // column name
                gen.Emit(OpCodes.Call, stringEqualityOperator);
                gen.Emit(OpCodes.Brfalse_S, endOfIf);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldloc, forLoopI);
                gen.Emit(OpCodes.Stfld, indexFields[i]); // index
                gen.MarkLabel(endOfIf);
            }
            // }
            gen.MarkLabel(incrementForLoop);
            gen.Emit(OpCodes.Ldloc, forLoopI);
            gen.Emit(OpCodes.Ldc_I4_1);
            gen.Emit(OpCodes.Add);
            gen.Emit(OpCodes.Stloc, forLoopI);
            gen.MarkLabel(maybeRepeatForLoop);
            gen.Emit(OpCodes.Ldloc, forLoopI);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldlen);
            gen.Emit(OpCodes.Conv_I4);
            gen.Emit(OpCodes.Blt, repeatForLoop);

            // Done
            gen.Emit(OpCodes.Ret);
            return constructor;
        }

        private MethodBuilder BuildMethodCreateFromRow(
            TypeBuilder classBuilder,
            FieldBuilder[] indexFields,
            FieldBuilder[] columnNameFields,
            PropertyInfo[] propertiesOfT)
        {
            Type[] propertyTypes = propertiesOfT.Select(p => p.PropertyType).ToArray();

            // Create the method
            MethodBuilder method = classBuilder.DefineMethod("CreateFromRow",
                MethodAttributes.Public
               | MethodAttributes.HideBySig
               | MethodAttributes.NewSlot
               | MethodAttributes.Virtual
               | MethodAttributes.Final,
               CallingConventions.HasThis,
               typeof(T),
               new Type[] { typeof(string[]) }
            );

            ILGenerator gen = method.GetILGenerator();

            //J139Report obj = new J139Report();
            LocalBuilder objLocal = gen.DeclareLocal(typeof(T));
            ConstructorInfo constructorOfT = typeof(T).GetConstructor(Type.EmptyTypes);
            gen.Emit(OpCodes.Newobj, constructorOfT);
            gen.Emit(OpCodes.Stloc, objLocal);

            // Repeat code for each property
            for (int i = 0; i < indexFields.Length; i++)
            {
                //if (indexClientId >= 0)
                //{
                Label ifIndexGreaterThanZeroLabel = gen.DefineLabel();

                //    obj.NumberField = Convert.ToInt32(csvValue[indexNumberField]);
                //    obj.DateField = Convert.ToDateTime(csvValue[indexDateField]);
                //    obj.StringField = csvValue[indexStringField];
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, indexFields[i]);
                gen.Emit(OpCodes.Ldc_I4_0);
                gen.Emit(OpCodes.Blt_S, ifIndexGreaterThanZeroLabel);
                gen.Emit(OpCodes.Ldloc_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, indexFields[i]);
                gen.Emit(OpCodes.Ldelem_Ref);
                Type currentPropertyType = propertiesOfT[i].PropertyType;

                MethodInfo converter = converterProvider.GetStringConverterTo(currentPropertyType);
                gen.Emit(OpCodes.Call, converter);
                gen.Emit(OpCodes.Callvirt, propertiesOfT[i].GetSetMethod());


                gen.MarkLabel(ifIndexGreaterThanZeroLabel);
                //}
            }

            gen.Emit(OpCodes.Ldloc, objLocal);
            gen.Emit(OpCodes.Ret);

            return method;
        }
    }
}
