using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class TaskTest : MonoBehaviour
{
    // '�񵿱�': '����'�� �ݴ븻�� � '�۾�'�� ������ �� �� �۾��� �Ϸ���� �ʾƵ�
    //           ���� �ڵ带 �����ϴ� ���
    // �� '�۾�'�� Ư¡: �ð��� �����ɸ���. (ex. ���귮�� ���ų�, IO �۾� ��)


    private async Task Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 1. ����
            //LongLoop();

            // 2. �񵿱� (Coroutine)
            // StartCoroutine(LongLoop_Coroutine());

            // 3. �񵿱� (Task)
            // 3-1. ��ȯ���� ���� Task
            //Task task1 = new Task(LongLoop);
            //task1.Start();

            // 3-2. ��ȯ���� �ִ� Task
            Task<int> task2 = new Task<int>(LongLoop2);
            task2.Start();

            //task2.Wait(); // Task�� �Ϸ�� ������ ��ٸ�
            //int result = task2.Result; // Task�� ����� ������
            //Debug.Log("�۾� ���: " + result);

            // ContinueWith�� �̿��� �ݹ� ���
            //task2.ContinueWith(t =>
            //{
            //    // Task�� �Ϸ�� �� ������ �ڵ�
            //    Debug.Log("�۾� ���: " + t.Result);
            //});

            // �񵿱⸦ ����ó�� �����ϱ� ���� ����� Ű���尡 async + await
            int result = await task2;
            Debug.Log("�۾� ���: " + result);
        }
    }


    // ���귮�� ���� �۾�
    private void LongLoop()
    {
        long sum = 1;
        for (long i = 1; i < 10000000000; ++i)
        {
            sum *= i;
        }
        Debug.Log("�۾� �Ϸ�");
        // Task�� �̿��� ȣ�⿡�� �Ʒ� MonoBehaviour�� ��ӹ޴� 
    }

    private int LongLoop2()
    {
        long sum = 1;
        for (long i = 1; i < 10000000000; ++i)
        {
            sum *= i;
        }
        Debug.Log("�۾� �Ϸ�");
        return 32423;
    }

    private IEnumerator LongLoop_Coroutine()
    {
        long sum = 1;
        for (long i = 1; i < 10000000000; ++i)
        {
            sum *= i;
            if (i % 1000 == 0)
            {
                yield return null;
            }
        }
        Debug.Log(sum);
    }
}