import React = require("react");
import Models = require("./Models");

class StockData extends React.Component<Models.StockSymbol, {}> {
    constructor(props: Readonly<Models.StockSymbol>) {
        super(props);
        //this.state = {symbol: null};
    }
}

export = StockData;