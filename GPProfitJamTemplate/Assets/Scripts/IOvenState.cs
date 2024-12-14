public interface IOvenState
{
    void Handle(Oven oven);
}

public class EmptyState : IOvenState
{
    public void Handle(Oven oven)
    {
        if (oven.hasMixedProduct)
        {
            // Переходим в состояние запекания только один раз
            oven.isBaking = true;
            oven.ChangeState(new BakingState());
            SoundManager.Instance.PlaySound("BakingSound");
            oven.ovenSlider.gameObject.SetActive(true);
        }
    }
}

public class BakingState : IOvenState
{
    public void Handle(Oven oven)
    {
        oven.Baking();

        //Запекание завершено
        if(!oven.isBaking)
        { 
            oven.isBaking = false;
            oven.hasBakedDish = true;
            oven.hasMixedProduct = false;
            oven.ChangeState(new BakedState());

            SoundManager.Instance.PlaySound("BakedSound");
            oven.ovenSlider.gameObject.SetActive(false);
        }
    }
}


public class BakedState : IOvenState
{
    public void Handle(Oven oven)
    {

    }
}
