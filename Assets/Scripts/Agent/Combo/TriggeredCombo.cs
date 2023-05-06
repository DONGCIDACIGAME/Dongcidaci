public class TriggeredCombo
{
    private Combo combo;
    private int triggeredAt;

    public ComboStep GetCurrentStep()
    {
        return combo.comboSteps[triggeredAt];
    }

    public bool MoveNext(byte inputType)
    {
        if(triggeredAt >= combo.comboSteps.Length-1)
        {
            return false;
        }

        ComboStep nextStep = combo.comboSteps[triggeredAt + 1];
        if(inputType == nextStep.input)
        {
            triggeredAt++;
            return true;
        }

        return false;
    }
}
