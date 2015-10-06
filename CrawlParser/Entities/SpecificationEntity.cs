using System;
using System.ComponentModel;

namespace CrawlParser
{
    /// <summary>
    /// Individual product specification.
    /// </summary>
    public class SpecificationEntity
    {
        private Types _specType;

        public SpecificationEntity(string label, string value)
        {
            SetSpectType(label);
            this.Label = label;
            this.Value = value;
        }

        /// <summary>
        /// The specification types we are most interested in.
        /// </summary>
        public enum Types
        {
            [Description("Depth")]
            DEPTH,
            [Description("Length")]
            LENGTH,
            [Description("Height")]
            HEIGHT,
            [Description("Width")]
            WIDTH,
            [Description("Shape")]
            SHAPE,
            [Description("Center Size")]
            CENTER_SIZE,
            [Description("Material")]
            MATERIAL,
            [Description("Collection")]
            COLLECTION,
            [Description("Color/Finish")]
            COLOR_FINISH,
            [Description("Faucet Type")]
            FAUCET_TYPE,
            [Description("Faucet Connection")]
            FAUCET_CONNECTION,
            [Description("Flow Rate")]
            FLOW_RATE,
            [Description("Number Of Bowls")]
            NUMBER_OF_BOWLS,
            [Description("Drain Opening")]
            DRAIN_OPENING,
            [Description("Drain Location")]
            DRAIN_LOCATION,
            [Description("Overall Sink Size")]
            OVERALL_SINK_SIZE,
            [Description("Bowl Size")]
            BOWL_SIZE,
            [Description("ADA Compliant")]
            ADA_COMPLIANT,
            [Description("ASME Specifications")]
            ASME_SPECIFICATIONS,
            [Description("Application")]
            APPLICATION,
            [Description("CSA Certified")]
            CSA_CERTIFIED,
            [Description("Brand / Model Compatibility")]
            BRAND_MODEL_COMP,
            [Description("Cartridge Type")]
            CARTRIDGE_TYPE,
            [Description("Installation Type")]
            INSTALLATION_TYPE,
            [Description("Handle Type")]
            HANDLE_TYPE,
            [Description("Spray Type")]
            SPRAY_TYPE,
            [Description("Faucet Installation")]
            FAUCET_INSTALLATION,
            [Description("WaterSense Labeled")]
            WATER_SENSE_LABELED,
            [Description("Spout Reach")]
            SPOUT_REACH,
            [Description("Spout Type")]
            SPOUT_TYPE,
            [Description("Unknown Type")]
            UNKNOWN
        }

        /// <summary>
        /// The label (Ex. Ada Compliant).
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The value (ex. true).
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The specification type.
        /// </summary>
        public Types Spec_Type
        {
            get { return _specType; }
        }

        /// <summary>
        /// Get the total number of known types.
        /// </summary>
        /// <returns></returns>
        public int GetTypeCount()
        {
            return Enum.GetNames(typeof(Types)).Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        private void SetSpectType(string label)
        {
            switch (label)
            {
                case "ADA Compliant":
                    _specType = Types.ADA_COMPLIANT;
                    break;
                case "ASME Specifications":
                    _specType = Types.ASME_SPECIFICATIONS;
                    break;
                case "Application":
                    _specType = Types.APPLICATION;
                    break;
                case "Collection":
                    _specType = Types.COLLECTION;
                    break;
                case "Color/Finish":
                case "Color/Finish Category":
                    _specType = Types.COLOR_FINISH;
                    break;
                case "CSA Certified":
                    _specType = Types.CSA_CERTIFIED;
                    break;
                case "Center Size":
                    _specType = Types.CENTER_SIZE;
                    break;
                case "Brand / Model Compatibility":
                    _specType = Types.BRAND_MODEL_COMP;
                    break;
                case "Cartridge Type":
                    _specType = Types.CARTRIDGE_TYPE;
                    break;
                case "Faucet Type":
                    _specType = Types.FAUCET_TYPE;
                    break;
                case "Faucet Connection":
                    _specType = Types.FAUCET_CONNECTION;
                    break;
                case "Flow Rate":
                    _specType = Types.FLOW_RATE;
                    break;
                case "Installation Type":
                    _specType = Types.INSTALLATION_TYPE;
                    break;
                case "Depth":
                    _specType = Types.DEPTH;
                    break;
                case "Length":
                    _specType = Types.LENGTH;
                    break;
                case "Handle Type":
                    _specType = Types.HANDLE_TYPE;
                    break;
                case "Material":
                    _specType = Types.MATERIAL;
                    break;
                case "Height":
                    _specType = Types.HEIGHT;
                    break;
                case "Spray Type":
                    _specType = Types.SPRAY_TYPE;
                    break;
                case "Faucet Installation":
                    _specType = Types.FAUCET_INSTALLATION;
                    break;
                case "WaterSense Labeled":
                    _specType = Types.WATER_SENSE_LABELED;
                    break;
                case "Spout Reach":
                    _specType = Types.SPOUT_REACH;
                    break;
                case "Spout Type":
                    _specType = Types.SPOUT_TYPE;
                    break;
                case "Number Of Bowls":
                    _specType = Types.NUMBER_OF_BOWLS;
                    break;
                case "Drain Opening":
                    _specType = Types.DRAIN_OPENING;
                    break;
                case "Drain Location":
                    _specType = Types.DRAIN_LOCATION;
                    break;
                case "Overall Sink Size":
                    _specType = Types.OVERALL_SINK_SIZE;
                    break;
                case "Width":
                    _specType = Types.WIDTH;
                    break;
                case "Shape":
                    _specType = Types.SHAPE;
                    break;
                case "Bowl Size Single Or Left":
                case "Right Bowl Size":
                    _specType = Types.BOWL_SIZE;
                    break;


                default:
                    _specType = Types.UNKNOWN;
                    break;
            }
        }
    }
}
