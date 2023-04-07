/// <summary>
/// TODO:后续根据连招的设计重写
/// 连招的触发规则？
/// 如果有空拍，怎么处理
/// </summary>
public class ComboHandler:  IMeterHandler
{
    //private int[] comboMeter = new int[] { };
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
