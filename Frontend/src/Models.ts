export interface StockValue {
    TimeStamp: Date;
    Open: number;
    High: number;
    Low: number;
    Close: number;
    Volume: number;
}

export interface StockSymbol {
    Id: string;
    Symbol: string;
    Added: Date;
    LastRefresh: Date;
    Values: Array<StockValue>;
}