using System;
using System.Collections;
using UnityEngine;
using UnityEngine.GameFoundation;
using UnityEngine.GameFoundation.DefaultLayers;
using UnityEngine.Promise;

public class GFInit : MonoBehaviour
{
    IEnumerator Start()
    {
        // Creates a new data layer for Game Foundation,
        // with the default parameters.
        MemoryDataLayer dataLayer = new MemoryDataLayer();

        // - Initializes Game Foundation with the data layer.
        // - We use a using block to automatically release the deferred promise handler.
        using (Deferred initDeferred = GameFoundationSdk.Initialize(dataLayer))
        {
            yield return initDeferred.Wait();

            if (initDeferred.isFulfilled)
                OnInitSucceeded();
            else
                OnInitFailed(initDeferred.error);
        }
    }

    // Called when Game Foundation is successfully initialized.
    void OnInitSucceeded()
    {
        Debug.Log("Game Foundation is successfully initialized");

        // Use the key you've used in the previous tutorial.
        const string definitionKey = "diamondsCurrency";

        // Finding a currency takes a non-null string parameter.
        Currency definition = GameFoundationSdk.catalog.Find<Currency>(definitionKey);

        // Make sure you retrieved a valid currency.
        if (definition is null)
        {
            Debug.Log($"Definition {definitionKey} not found");
            return;
        }

        // You can get the balance of a currency with the WalletManager.
        long balance = GameFoundationSdk.wallet.Get(definition);

        Debug.Log($"The balance of '{definition.displayName}' is {balance.ToString()}");
    }

    // Called if Game Foundation initialization fails
    void OnInitFailed(Exception error)
    {
        Debug.LogException(error);
    }
}
