using UnityEngine;
using System.Collections;

public class BombCasingGen : MonoBehaviour {

    public GameObject Bomb_Backing;
    public GameObject Empty_Bomb;
    public GameObject Cross_Bar;

    public int size;
    public float offset = 0.22f;

    public void BuildCasings()
    {
        float half = size / 2.0f;
        GameObject bomb = GameObject.Instantiate(Empty_Bomb);
        bomb.name = size + "x" + size + " Casing (" + (size * size * 2 -1) + " modules)";
        Casing casing = bomb.GetComponent<Casing>();
        Transform visual_transform = casing.Visual;
        KMBombFace front_face = casing.Front.GetComponent<KMBombFace>();
        KMBombFace rear_face = casing.Back.GetComponent<KMBombFace>();

        front_face.Anchors = new System.Collections.Generic.List<Transform>();
        front_face.Backings = new System.Collections.Generic.List<KMModuleBacking>();
        front_face.GetComponent<KMSelectable>().ChildRowLength = size;
        rear_face.Anchors = new System.Collections.Generic.List<Transform>();
        rear_face.Backings = new System.Collections.Generic.List<KMModuleBacking>();
        rear_face.GetComponent<KMSelectable>().ChildRowLength = size;

        casing.Distance_Collider.size = new Vector3(size * 0.23f, 0.20f, size * 0.23f);
        casing.Selectable_Area.size = new Vector3(size * 0.24f, size * 0.24f, 0.22f);
        casing.Selectable_Area.transform.Translate(0, -0.25f, 0);

        casing.Highlight.localScale = new Vector3(size * 0.24f, size * 0.24f, 0.22f);

        // casing.Body.localScale = new Vector3(size * 0.23f, 0.18f, size * 0.23f);

        float crossbar_width = 0.025f;
        float widget_offset = 0.22f;
        float widget_constant_offset = crossbar_width + 0.00275f;

        //Make the widget anchors
        for (int w = 0; w < size; w++)
        {
            Transform Bface = new GameObject().GetComponent<Transform>();
            Bface.Translate(new Vector3(offset * (w - half + 0.5f), 0.0f, 0.0f));
            Bface.Rotate(-90, 0, 0);
            Bface.SetParent(casing.W_Bottom);
            Bface.localScale = new Vector3(0.12f, 0.03f, 0.17f);
            Bface.name = "Bottom Face";
            bomb.GetComponent<KMBomb>().WidgetAreas.Add(Bface.gameObject);

            Transform Tface = new GameObject().GetComponent<Transform>();
            Tface.Translate(new Vector3(offset * (w - half + 0.5f), 0.0f, 0.0f));
            Tface.Rotate(-90, 180, 0);
            Tface.SetParent(casing.W_Top);
            Tface.localScale = new Vector3(0.12f, 0.03f, 0.17f);
            Tface.name = "Top Face";
            bomb.GetComponent<KMBomb>().WidgetAreas.Add(Tface.gameObject);

            Transform Lface = new GameObject().GetComponent<Transform>();
            Lface.Translate(new Vector3(0.0f, 0.0f, offset * (w - half + 0.5f)));
            Lface.Rotate(-90, 90, 0);
            Lface.SetParent(casing.W_Left);
            Lface.localScale = new Vector3(0.12f, 0.03f, 0.17f);
            Lface.name = "Left Face";
            bomb.GetComponent<KMBomb>().WidgetAreas.Add(Lface.gameObject);

            Transform Rface = new GameObject().GetComponent<Transform>();
            Rface.Translate(new Vector3(0.0f, 0.0f, offset * (w - half + 0.5f)));
            Rface.Rotate(-90, -90, 0);
            Rface.SetParent(casing.W_Right);
            Rface.localScale = new Vector3(0.12f, 0.03f, 0.17f);
            Rface.name = "Right Face";
            bomb.GetComponent<KMBomb>().WidgetAreas.Add(Rface.gameObject);
        }

        casing.W_Bottom.Translate(new Vector3(0, 0, size * -widget_offset / 2 - widget_constant_offset),Space.World);
        casing.W_Top.Translate(new Vector3(0, 0, size * widget_offset / 2 + widget_constant_offset), Space.World);
        casing.W_Left.Translate(new Vector3(size * -widget_offset / 2 - widget_constant_offset, 0, 0), Space.World);
        casing.W_Right.Translate(new Vector3(size * widget_offset / 2 + widget_constant_offset, 0, 0), Space.World);


        //Generate the crossbars.
        for (int i = 0; i <= size; i++)
        {
            Transform CrossBar1 = (GameObject.Instantiate(Cross_Bar) as GameObject).GetComponent<Transform>();
            CrossBar1.SetParent(visual_transform);
            CrossBar1.localScale = new Vector3(crossbar_width, 0.21f, size * 0.22f);
            CrossBar1.Translate(new Vector3(offset * (i - half), 0, -0));

            Transform CrossBar2 = (GameObject.Instantiate(Cross_Bar) as GameObject).GetComponent<Transform>();
            CrossBar2.SetParent(visual_transform);
            CrossBar2.localScale = new Vector3(size * 0.22f, 0.22f, crossbar_width);
            CrossBar2.Translate(new Vector3(0, 0, offset * (i - half)));
        }

        // Generate The module backings and anchors
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                GameObject front_backing = GameObject.Instantiate(Bomb_Backing);    // Grab the prefab
                Transform f = front_backing.GetComponent<Transform>();
                f.SetParent(casing.Faces_F);
                f.Translate(new Vector3(offset * (x - half), offset * (y - half), -0.01f));
                f.Translate(new Vector3(-0.687f, 0.359f, 0.0f));
                f.name = "Bomb_Foam_" + x + "_" + y + "_F";
                Transform f_anchor = new GameObject().GetComponent<Transform>();    // We need to rotate the anchor relative to the backing, so we need a new transform
                f_anchor.position = f.position;
                f_anchor.parent = f;
                f_anchor.Translate(0, 0.08f, 0);    // Move the modules out of the backing
                f_anchor.Rotate(new Vector3(0, 0, 0));
                f_anchor.name = "Anchor";
                front_face.Anchors.Add(f_anchor);
                front_face.Backings.Add(front_backing.GetComponent<KMModuleBacking>());
                // And do it all again for the back face
                GameObject rear_backing = GameObject.Instantiate(Bomb_Backing);
                Transform r = rear_backing.GetComponent<Transform>();
                r.SetParent(casing.Faces_R);
                r.Translate(new Vector3(offset * (x - half), offset * (y - half), 0.01f));
                r.Translate(new Vector3(-0.687f, 0.359f, 0.0f));
                r.Rotate(new Vector3(0, 180, 0));
                r.name = "Bomb_Foam_" + x + "_" + y + "_R";
                Transform r_anchor = new GameObject().GetComponent<Transform>();
                r_anchor.position = r.position;
                r_anchor.parent = r;
                r_anchor.Translate(0, -0.08f, 0);
                r_anchor.Rotate(new Vector3(0, 0, 180));
                r_anchor.name = "Anchor";
                rear_face.Anchors.Add(r_anchor);
                rear_face.Backings.Add(rear_backing.GetComponent<KMModuleBacking>());
            }
        }
        bomb.GetComponent<KMBomb>().Scale = 2.2f / size;
    }

    public void build_all_casings()
    {
        for (int i = 2; i <=22; i++)
        {
            size = i;
            BuildCasings();
        }
    }

}
