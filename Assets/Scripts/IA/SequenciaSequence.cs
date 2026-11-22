using System;
using Unity.Behavior;
using UnityEngine;
using Composite = Unity.Behavior.Composite;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Sequencia", category: "Flow", id: "26d3337814c89edbff895a6f5c3c1093")]
public partial class SequenciaSequence : Composite
{


    protected override Status OnStart()
    {
        return Status.Running;
        
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

