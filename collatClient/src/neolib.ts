import Neon from '@cityofzion/neon-js';
// import { rpc, sc, tx, wallet, u, CONST, settings, logging } from "@cityofzion/neon-core";
// imports taken from the innards of neon-js because neon-js doesnt expose the types itself
import { wallet } from '@cityofzion/neon-core';
import api from '@cityofzion/neon-api/lib/plugin';
import { NotificationMessage } from '@cityofzion/neon-api/lib/notifications/responses';

class NeoApi {
  private apiProvider: api.neoscan.instance;

  private account: wallet.Account;

  private notifications: api.notifications.instance;

  constructor(
    private contractScriptHash: string,
    privateKey: string,
    network: 'MainNet'|'TestNet',
  ) {
    this.account = Neon.create.account(privateKey);
    this.apiProvider = new api.neoscan.instance(network);
    this.notifications = new api.notifications.instance(network);
  }

  public async invoke(operation:string, args:Array<string|number>) {
    const sb = Neon.create.scriptBuilder();
    // Contract script hash, function name and parameters
    sb.emitAppCall(this.contractScriptHash, operation, args);

    // Returns a hexstring
    const script = sb.str;

    const config = {
      api: this.apiProvider, // Network
      account: this.account, // Your Account
      script, // The Smart Contract invocation script
    };

    await Neon.doInvoke(config);
  }

  public subscribe(callback: (event:NotificationMessage)=>void) {
    return this.notifications.subscribe(this.contractScriptHash, callback);
  }

  get address(): string {
    return this.account.address;
  }
}

export { NotificationMessage };
export default NeoApi;
