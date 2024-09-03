import { CompanyTenK } from "../../../company";

interface Props {
  tenK: CompanyTenK;
}

const TenKFinderItem = ({ tenK }: Props) => {
  const fillingDate = new Date(tenK.fillingDate).getFullYear();
  return (
    <a
      href={tenK.finalLink}
      target="_blank"
      rel="noopener noreferrer"
      className="inline-flex items-center p-4 text-md text-white bg-lightGreen rounded-md"
    >
      10K - {tenK.symbol} - {fillingDate}
    </a>
  );
};

export default TenKFinderItem;
