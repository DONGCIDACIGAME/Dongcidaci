public class ComboHandler:  IMeterHandler
{
    private int[] comboMeter = new int[] { };
    private int[] comboAnimList = new int[] { 0, 1, 2, 3 };
    private int comboCounter = 0;

    public int GetAnimIndex()
    {
        int len = comboAnimList.Length;
        return comboCounter % len;
    }

    public void TriggerCombo()
    {
        comboCounter++;
    }

    public void OnUpdate(float deltaTime)
    {

    }

    public void OnMeter(int meterIndex)
    {
        
    }
}
