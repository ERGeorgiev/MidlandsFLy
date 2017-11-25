using System;
using System.Collections.Generic;

/// <summary>
/// This class is used for storting information about maintenance performed by each employee.
/// </summary>

public class MaintenanceHistory
{
    static List<string> desc = new List<string>();
    static Random random = new Random();

    private string regNumber;
    private string id;
    private string description;
    public static byte description_length = 255;

    public string Id { get => id; private set => id = value; }
    public string RegNumber { get => regNumber; private set => regNumber = value; }
    public string Description { get => description; private set => description = value; }

    public MaintenanceHistory(string regNumber, string id, string description)
    {
        this.RegNumber = regNumber;
        this.Id = id;
        this.Description = description;
    }

    public static string GenerateRandomDescription()
    {
        if (desc.Count == 0)
        {
            desc.Add("SERVICED THE BLUE HYDRAULIC SYSTEM RESERVOIR.");
            desc.Add("RECYCLED FLAPS AND SPEED BRAKES.");
            desc.Add("REPLACED LT MLG UPLOCK ACTUATOR.");
            desc.Add("CLEANED AND LUBED RT MLG UPLOCK HOOK ASSY.");
            desc.Add("RELAMPED EMERGENCY LIGHTING.");
            desc.Add("REPAIRED PER SWA RA 1-533-1232.");
            desc.Add("INSTALLED A NEW STRUT SUPPORT IAW DC-9 SRM 51-30-5 AND 51-30-2.");
            desc.Add("INTERMITTENT ENGINE NR 2 FIRE LIGHT ON CLIMBOUT OFF OF HBP. REPLACED FLAME DETECTOR/HARNESS ASSY.");
            desc.Add("ENGINE OVERHEAT FOR 1 SECOND DURING START UP. RETURNED TO GATE FOR MX INVESTIGATION. ENG IS RETURNED TO SERVICE WITH NO FURTHER TROUBLESHOOTING.");
            desc.Add("CHANGED T12 SENSOR FOR ENG 1 IAW AMNM 73-21-40-000-040. IDLE RUN CARRIED OUT.");
            desc.Add("CRACKED BELLOWS IN THE C6VW1026-3 BLEED AIR DUCT. DUCT WAS REPLACED & ACFT RETURNED TO SERVICE.");
            desc.Add("L1 SLIDE PRESSURE NOT VISABLE. CLEARED VIEWING GLASS.");
            desc.Add("INSPECTION TYPE: C12, MAIN CABIN FLOOR PANEL 241XMF IS DAMAGED IN MULTIPLE PLACES, FABRICATED NEW FLOOR PANEL IAW SREO B757-5320-9692-H.");
            desc.Add("FOUND TORQUE ON WINDSHIELD SCREW 1/4 TO 1/2 TURN LOOSE ON 40% OF SCREWS.");
            desc.Add("PERFORM ACC 1-3 RTS TEST IAW AMM MD 11-21-53-20-700-801, ALL PASSED, AC OK TO SERVICE.");
            desc.Add("DAMAGE TO PAX DOOR. REPAIRED THE DOOR SEAL RETAINERS.");
            desc.Add("NDC SHOWED FAULT HISTORY FOR THE RT PRSOV AND THE RT HPSOV. PERFORMED OPS TEST OF BOTH VALVES IAW THE AMM/ SYSTEM CHECKED GOOD.");
            desc.Add("TIE CLIP CRACKED AT BS 927, STRINGER 23L. R & R TIE CLIP IAW SRM.");
            desc.Add("AFT FUSELAGE SKIN CORRODED ALONG LOWER EDGE OF LAV PANEL. COMPLY WITH ERA 537873-14, REV O.");
            desc.Add("RESET FLAPS, LUBED BALL SCREWS & INSPECTED IAW MM. OPERATIONS CHECK WAS GOOD.");
            desc.Add("REMOVED CORROSION DAMAGE AREA LESS THAN 50%. FABRICATED DOUBLER & INSTALLED IAW SRM 53-04 FIG 57 WO 2355620 M301 242 WO 1011359 1000-0034.");
            desc.Add("APU IGNITER PLUG WORN BEYOND LIMITS. INSTALLED NEW APU IGNITER PLUG. OPERATIONS GOOD IAW AMM 49-41-02.");
            desc.Add("NUMBER 2 ENG CLAMP BELOW FAN AIR SOV IS MISSING CUSHION. REPLACED CLAMP IAW AMM 70-50-00.");
            desc.Add("CHAFING WEAR ON AFT FUEL DRAIN LINE LOCATED ON APU DRAIN TUBE CLUSTER. CLEANED & VERIFIED LIMITS AT AFT FUEL DRAIN LINE ON APU. FOUND IN LIMITS IAW AMM 20-20-05.");
            // Descriptions taken from: http://av-info.faa.gov/sdrx/Query.aspx
        }

        return desc[random.Next(0, desc.Count)];
    }

    //public MaintenanceHistory(string date, string description = "none", MaintenanceType maintenanceType = MaintenanceType.Unknown) : this(description, maintenanceType)
    //{
    //    if (DateTime.TryParse(date, out DateTime dateCheck) == true)
    //    {
    //        this.Date = dateCheck;
    //    }
    //    else
    //        throw new FormatException("Date must be entered in the following format: MM/dd/yyyy");
    //}
}