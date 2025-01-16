import React, { ChangeEvent, SyntheticEvent, useEffect, useState } from "react";
import { searchCompanies } from "../../api";
import { CompanySearch } from "../../company";
import CardList from "../../Components/CardList/CardList";
import Navbar from "../../Components/Navbar/Navbar";
import ListPortfolio from "../../Components/Portfolio/ListPortfolio/ListPortfolio";
import Search from "../../Components/Search/Search";
import { PortfolioGet } from "../../Models/Portfolio";
import { portfolioAddAPI, portfolioDeleteAPI, portfolioGetAPI } from "../../Services/PortfolioService";
import { toast } from "react-toastify";

interface Props {}

const SearchPage = (props: Props) => {
  //using generic to pass int the type of useState explicitly
  const [search, setSearch] = useState<string>(""); //search is the getter. setSearch is the setter.
  //can't store data in a let and store in setSearch whilst trying to access data from api call,
  //because of type mismatching (a problem you dont get in javascript)
  const [searchResult, setSearchResult] = useState<CompanySearch[]>([]);
  const [serverError, setServerError] = useState<string>("");
  const [portfolioValues, setPortfolioValues] = useState<PortfolioGet[] | null>([]);

  const handleSearchChange = (e: ChangeEvent<HTMLInputElement>) => {
    setSearch(e.target.value);
  };

  useEffect(() => {
    getPortfolio();
  }, []);

  const getPortfolio = () => {
    portfolioGetAPI()
    .then((res) => {
      if(res?.data) {
        setPortfolioValues(res?.data);
      }
    }).catch((e) => {
      toast.warning("Could not get portfolio values!");
    });
  };

  const onSearchSubmit = async (e: SyntheticEvent) => {
    e.preventDefault();
    const result = await searchCompanies(search);
    //type narrowing
    if (typeof result === "string") {
      setServerError(result);
    } else if (Array.isArray(result.data)) {
      setSearchResult(result.data);
    }
  };

  const onPortfolioCreate = (e: any) => {
    e.preventDefault();
    portfolioAddAPI(e.target[0].value)
    .then((res) => {
      if(res?.status === 204) {
        toast.success("Stock added to portfolio!");
        getPortfolio();
      }
    }).catch((e) => {
      toast.warning("Could not create porfolio item!");
    })
  };

  const onPortfolioDelete = (e: any) => {
    e.preventDefault(); //ensures page doesnt load when we submit and blow away all the data
    portfolioDeleteAPI(e.target[0].value)
    .then((res) => {
      if(res?.status === 200) {
        toast.success("stock deleted from portfolio!");
        getPortfolio();
      }
    }).catch((e) => {
      toast.warning("Could not delete portfolio");
    })
  };


  return (
    <div className="App">
      <Search
        search={search}
        handleSearchChange={handleSearchChange}
        onSearchSubmit={onSearchSubmit}
      />
      {serverError && <h1>{serverError}</h1>}
      <ListPortfolio
        portfolioValues={portfolioValues!}
        onPortfolioDelete={onPortfolioDelete}
      />
      <CardList
        searchResults={searchResult}
        onPortfolioCreate={onPortfolioCreate}
      />
    </div>
  );
};

export default SearchPage;
