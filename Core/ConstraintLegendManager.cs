using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConstraintLegendManager : MonoBehaviour {

    public List<RectTransform> constraintInfos = new List<RectTransform>();

    [Header("Manual")]
    public RectTransform constraintInfoPrototype;

    private void GenerateConstraintCard(Constraint constraint) {
        RectTransform constraintCard = Instantiate(constraintInfoPrototype, transform);
        constraintCard.transform.position = constraintInfoPrototype.transform.position;
        constraintCard.transform.position -= Vector3.up * (constraintCard.rect.height) * constraintInfos.Count;
        constraintCard.gameObject.SetActive(true);
        constraintInfos.Add(constraintCard);

        // Set Letter
        TextMeshProUGUI constraintLetter = constraintCard.transform.Find("ConstraintLetter").GetComponent<TextMeshProUGUI>();
        constraintLetter.text = SText.Format($"{constraint.letter}");

        // Set Text
        TextMeshProUGUI constraintText = constraintCard.transform.Find("ConstraintText").GetComponent<TextMeshProUGUI>();
        constraintText.text = SText.Format($"{constraint.GetDescription()}");
    }

    public void DisplayConstraintInfos(List<Constraint> constraints) {
        foreach (RectTransform rt in constraintInfos) {
            Destroy(rt.gameObject);
        }
        constraintInfos.Clear();
        foreach (Constraint constraint in constraints) {
            //if(!(constraint is StepParentConstraint))
            GenerateConstraintCard(constraint);
        }
    }

}
