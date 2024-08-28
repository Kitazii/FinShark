import React, { ChangeEvent, SyntheticEvent, useState } from "react";
import { searchCompanies } from "../../api";
import { CompanySearch } from "../../company";
import CardList from "../../Components/CardList/CardList";
import Navbar from "../../Components/Navbar/Navbar";
import ListPortfolio from "../../Components/Portfolio/ListPortfolio/ListPortfolio";
import Search from "../../Components/Search/Search";

interface Props {}

const SearchPage = (props: Props) => {
  //using generic to pass int the type of useState explicitly
  const [search, setSearch] = useState<string>(""); //search is the getter. setSearch is the setter.
  //can't store data in a let and store in setSearch whilst trying to access data from api call,
  //because of type mismatching (a problem you dont get in javascript)
  const [searchResult, setSearchResult] = useState<CompanySearch[]>([]);
  const [serverError, setServerError] = useState<string>("");
  const [portfolioValues, setPortfolioValues] = useState<string[]>([]);

  const handleSearchChange = (e: ChangeEvent<HTMLInputElement>) => {
    setSearch(e.target.value);
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
    const exists = portfolioValues.find((value) => value === e.target[0].value);
    if (exists) return;
    //going to spread the array
    //so that the different arrays are stored in different locations in memory (immutable)
    const updatedPortfolio = [...portfolioValues, e.target[0].value];
    setPortfolioValues(updatedPortfolio); //creates a new array for us, instead of using a push or enqueue data
  };

  const onPortfolioDelete = (e: any) => {
    e.preventDefault(); //ensures page doesnt load when we submit and blow away all the data
    //immutable method to get all the values from our portfolio
    //Any type of removal or update, the value must be immutable
    console.log(e.target[0].value);
    const removed = portfolioValues.filter((value) => {
      return value !== e.target[0].value;
    });
    setPortfolioValues(removed);
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
        portfolioValues={portfolioValues}
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
