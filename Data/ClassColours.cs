namespace GPSCompassClient.Data; 

public class ClassColours {

    public static string GetColourForClass(string id) {
        int code = id.GetHashCode();
        
        Random random = new(code);
        
        int r = random.Next(0, 255);
        int g = random.Next(0, 255);
        int b = random.Next(0, 255);
        
        return $"#{r:X2}{g:X2}{b:X2}";
    }
    
}