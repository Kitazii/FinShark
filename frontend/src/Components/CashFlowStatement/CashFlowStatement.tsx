import { useOutletContext } from "react-router";
import { CompanyCashFlow } from "../../company";
import { useEffect, useState } from "react";
import { getCashFlowStatement } from "../../api";
import Table from "../Table/Table";
import Spinner from "../Spinner/Spinner";

interface Props {}

const configs = [
  {
    label: "Date",
    render: (company: CompanyCashFlow) => company.date,
  },
  {
    label: "Operating Cashflow",
    render: (company: CompanyCashFlow) => company.operatingCashFlow,
  },
  {
    label: "Investing Cashflow",
    render: (company: CompanyCashFlow) =>
      company.netCashUsedForInvestingActivites,
  },
  {
    label: "Financing Cashflow",
    render: (company: CompanyCashFlow) =>
      company.netCashUsedProvidedByFinancingActivities,
  },
  {
    label: "Cash At End of Period",
    render: (company: CompanyCashFlow) => company.cashAtEndOfPeriod,
  },
  {
    label: "CapEX",
    render: (company: CompanyCashFlow) => company.capitalExpenditure,
  },
  {
    label: "Issuance Of Stock",
    render: (company: CompanyCashFlow) => company.commonStockIssued,
  },
  {
    label: "Free Cash Flow",
    render: (company: CompanyCashFlow) => company.freeCashFlow,
  },
];

const CashFlowStatement = (props: Props) => {
  const ticker = useOutletContext<string>();
  const [cashflowData, setCashflow] = useState<CompanyCashFlow[]>();
  useEffect(() => {
    const CashFlowStatementFetch = async () => {
      const result = await getCashFlowStatement(ticker!);
      setCashflow(result!.data);
    };
    CashFlowStatementFetch();
  }, []);
  return (
    <>
      {cashflowData ? (
        <>
          <Table config={configs} data={cashflowData} />
        </>
      ) : (
        <Spinner />
      )}
    </>
  );
};

export default CashFlowStatement;
