const fs = require('fs');

const menu = {
  'Home': '/'
};

module.exports = function (dest) {
  const currentUrl = getCurrentUrl(dest);
  console.log(`Rendering ${currentUrl}`);
  try {
    const viewModel = {
      name: 'Stryker.NET - mutation testing framework',
      tagline: 'Measure the effectiveness of your tests. Coming soon to a testrunner near you...',
      selectedMenuItem: selectedMenuItem(currentUrl),
      menu: menu,
    };
    return viewModel;
  } catch (err) {
    console.log(err);
    throw err;
  }
};

function selectedMenuItem(currentUrl) {
    const menuItem = Object.keys(menu).find(item => menu[item] === currentUrl);
    if (!menuItem) {
      throw Error(`No menu item found for ${currentUrl} in menu ${JSON.stringify(menu)}.`);
    }
    return menuItem;
}

function getCurrentUrl(dest) {
  if (dest === 'index.html') {
    return '/';
  } else {
    return `/${dest}`;
  }
}