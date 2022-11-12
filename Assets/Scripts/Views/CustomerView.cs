using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerView : MonoBehaviour, ICustomerView
{
    ICustomerPresenter presenter;

    [SerializeField]
    Slider waitSlider;

    [SerializeField]
    Transform orderParent;

    [SerializeField]
    Transform body;

    [SerializeField]
    List<GameObject> orderGo;

    Animator animator;

    Coroutine waitOrderCoroutine;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

    }
    private void Start()
    {
        presenter = GetComponent<CustomerPresenter>();

        orderGo = new List<GameObject>();
    }

    //Memulai slider menunggu order selesai dibuat
    private IEnumerator StartWaitSlider(float time)
    {
        waitSlider.gameObject.SetActive(true);
        waitSlider.maxValue = time;
        waitSlider.value = time;

        float counter = time;

        while(counter >= 0)
        {
            yield return new WaitForSeconds(1f);

            waitSlider.value = counter;
            counter--;

            if (counter < time / 3 && counter > 1f)
                PlaySadAnimation();
        }

        Return();
    }

    //Menampilkan daftar mamkanan yang di order
    public void Order(List<Menu> orders, float waitTime)
    {
        DeleteChild();

        if (this.waitOrderCoroutine != null)
            StopCoroutine(this.waitOrderCoroutine);

        this.waitOrderCoroutine = StartCoroutine(StartWaitSlider(waitTime));

        for(int i = 0; i < orders.Count; i++)
        {
            GameObject go = Instantiate(orders[i].MenuPrefab, orderParent);

            orderGo.Add(go);

            Vector3 pos = new Vector3(0, -1.2f * i, 0);
            go.transform.localPosition = pos;
        }
    }

    //Mengapus gameobject
    void DeleteChild()
    {
        foreach(GameObject go in orderGo)
        {
            Destroy(go);
        }

        orderGo.Clear();
    }

    //Menentukan arah character menghadap
    public void Turn()
    {
        Vector3 scale = body.localScale;
        scale.x *= -1;
        body.localScale = scale;
    }

    //kembali ke rumah ketika tidak mendapatkan makanan yang di order
    public void Return()
    {
        ShowOrder(false);

        presenter.Return();
    }

    public void ShowOrder(bool isActive)
    {
        orderParent.gameObject.SetActive(isActive);

        waitSlider.gameObject.SetActive(isActive);
    }

    //Memulai animasi berjalan
    public void PlayWalkAnimation(float velocity)
    {
        animator.SetFloat("Velocity", velocity);
    }

    //Memulai animasi ekspresi senang
    public void PlayHappyAnimation(float happyTime)
    {
        StartCoroutine(HappyAnimation(happyTime));
    }
    
    IEnumerator HappyAnimation(float happyTime)
    {
        animator.SetTrigger("Happy");
        yield return new WaitForSeconds(happyTime);
        animator.SetTrigger("Happy");
    }

    //Memulai animasi ekspresi sedih
    public void PlaySadAnimation()
    {
        animator.SetTrigger("Sad");
    }
}
